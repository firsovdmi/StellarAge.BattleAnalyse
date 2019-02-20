using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using StellarAge.BattleAnalyse.Log;
using StellarAge.BattleAnalyse.Model.Battle;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Ships;
using StellarAge.BattleAnalyse.Model.Turrels;
using StellarAge.BattleAnalyse.ViewModel;

namespace StellarAge.BattleAnalyse.Services
{
    public class BattleService
    {
        public List<UnitsView> GetDefaultShipList()
        {
            var ret = GetDefaultUnitsView(typeof(Ship));
            return ret;
        }
        public List<UnitsView> GetDefaultTurrel()
        {
            var ret = GetDefaultUnitsView(typeof(Turrel));
            return ret;
        }
        public List<UnitsView> GetDefaultUnitsView(Type unitType)
        {
            var ret = new List<UnitsView>();
            var units = GetDefaultUnits(unitType);
            foreach (var unit in units)
            {
                var viewItem = new UnitsView
                {
                    Name = unit.Name,
                    Weight = unit.Weight,
                    ClassName = unit.GetType().Name
                };
                ret.Add(viewItem);
            }
            return ret;
        }

        List<Unit> GetDefaultUnits(Type unitType)
        {
            var itemTypes = Assembly
                .GetAssembly(unitType)
                .GetTypes()
                .Where(t => t.IsSubclassOf(unitType))
                .ToList();
            var ret = itemTypes.Select(p => (Unit)Activator.CreateInstance(p)).ToList();
            return ret.OrderBy(p => p.Weight).ToList();
        }

        public LogBattle ExecuteBattle(BattleSettingsItem battleSettingsItem)
        {
            var battle = new Battle();
            battle.AttackHands = GetHandsFromViewGroups(battleSettingsItem.AttackHands);
            battle.DefenceHands = GetHandsFromViewGroups(battleSettingsItem.DefenceHands);
            battle.DefenceTurrelsGroups = GetUnitGroupsFromViewGroups(battleSettingsItem.DefenceTurrels);

            var ret = battle.SimBattle();
            return ret;
        }

        private List<Hand> GetHandsFromViewGroups(List<HandView> hands)
        {
            var ret = new List<Hand>();
            foreach (var hand in hands)
            {
                var newHand = new Hand { UnitGroups = GetUnitGroupsFromViewGroups(hand.UnitsView) };
                ret.Add(newHand);
            }
            return ret;
        }

        List<Unit> GetUnitGroupsFromViewGroups(List<UnitsView> unitsView)
        {
            var ret = new List<Unit>();
            var goodViews = unitsView.Where(p => p.Count > 0 && p.UnitAttackPower > 0 && p.UnitArmor > 0).ToList();
            foreach (var unitView in goodViews)
            {
                var unitsGroup = GetUnitsFromView(unitView);
                ret.Add(unitsGroup);
            }
            return ret;
        }
        Unit GetUnitsFromView(UnitsView unitView)
        {
            var unitType = Assembly
                .GetAssembly(GetType())
                .GetTypes().FirstOrDefault(t => t.Name == unitView.ClassName);
            if (unitType == null) return null;
            var newUnit = (Unit)Activator.CreateInstance(unitType);
            Mapper.Map(unitView, newUnit);
            return newUnit;
        }

        public LogBattle Optimize(BattleSettingsItem battleSettingsItem)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var threadCount = battleSettingsItem.ThreadCount;
            var quantumWeight = battleSettingsItem.HandWeight / (battleSettingsItem.Precision == 0 ? 1 : battleSettingsItem.Precision);
            var attackHandPattern = battleSettingsItem.AttackHands[0];

            var quantumList = new List<List<UnitsView>>();
            foreach (var paternUnit in attackHandPattern.UnitsView.Where(p => p.Weight > 0).Where(p => p.Count > 0))
            {
                var unitsPerQuant = (int)Math.Max(quantumWeight / paternUnit.Weight, 1);
                var quantumPerHandCount = (int)(battleSettingsItem.HandWeight / (unitsPerQuant * paternUnit.Weight));
                var quantumFromExistsUnitsCount = (int)paternUnit.Count / unitsPerQuant;
                var quantumCount = Math.Min(quantumPerHandCount, quantumFromExistsUnitsCount);
                var quantumListItem = new List<UnitsView>();
                for (var i = 0; i < quantumCount; i++)
                {
                    var quantomUnit = new UnitsView
                    {
                        Count = unitsPerQuant * (i + 1),
                        ClassName = paternUnit.ClassName,
                        UnitAttackPower = paternUnit.UnitAttackPower,
                        UnitArmor = paternUnit.UnitArmor,
                        Weight = paternUnit.Weight
                    };
                    quantumListItem.Add(quantomUnit);
                }
                quantumList.Add(quantumListItem);
            }

            var combinations = new List<List<UnitsView>>();
            for (var i = 0; i < quantumList.Count; i++)
            {
                Combine(quantumList, new List<UnitsView>(), battleSettingsItem.HandWeight, combinations, i);
            }
            var defenceHands = GetHandsFromViewGroups(battleSettingsItem.DefenceHands);
            var defenceTurrelsGroups = GetUnitGroupsFromViewGroups(battleSettingsItem.DefenceTurrels);
            var bestResult = decimal.MaxValue;
            var bestIndex = 0;




            var itemsForLastThread = combinations.Count % threadCount;
            var itemsPerThread = (combinations.Count - itemsForLastThread) / threadCount;
            var tasksList = new Task<(decimal BestResult, int BestIndex)>[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                var startIndex = i * itemsPerThread;
                var endIndex = startIndex + (itemsPerThread - 1);
                if (i >= threadCount) endIndex += itemsForLastThread;
                var newTask = ProcessParth(combinations, startIndex, endIndex, GetHandsFromViewGroups(battleSettingsItem.DefenceHands), GetUnitGroupsFromViewGroups(battleSettingsItem.DefenceTurrels));
                tasksList[i] = newTask;
            }
            Task.WaitAll(tasksList);
            foreach (var task in tasksList)
            {
                if (task.Result.BestResult >= bestResult) continue;
                bestResult = task.Result.BestResult;
                bestIndex = task.Result.BestIndex;
            }







            //for (var i = 0; i < combinations.Count; i++)
            //{
            //    var attackHands = new List<Hand> { new Hand { UnitGroups = GetUnitGroupsFromViewGroups(combinations[i]) } };
            //    var result = Battle.FastSim(attackHands, defenceHands, defenceTurrelsGroups);

            //    if (result >= bestResult) continue;
            //    bestResult = result;
            //    bestIndex = i;
            //}









            var battle = new Battle
            {
                AttackHands =
                    new List<Hand> { new Hand { UnitGroups = GetUnitGroupsFromViewGroups(combinations[bestIndex]) } },
                DefenceHands = defenceHands,
                DefenceTurrelsGroups = defenceTurrelsGroups
            };
            var ret = battle.SimBattle();
            watch.Stop();
            ret.Description = $"Проведено симуляций: {combinations.Count} за {watch.ElapsedMilliseconds} мс.";
            return ret;
        }

        private Task<(decimal BestResult, int BestIndex)> ProcessParth(
            List<List<UnitsView>> combinations
            , int startIndex
            , int endIndex
            , List<Hand> defenceHands
            , List<Unit> defenceTurrelsGroups)
        {
            var ret = Task<(decimal BestResult, int BestIndex)>.Factory.StartNew(() =>
            {
                var bestResult = decimal.MaxValue;
                var bestIndex = 0;
                for (var i = startIndex; i <= endIndex; i++)
                {
                    var attackHands = new List<Hand> { new Hand { UnitGroups = GetUnitGroupsFromViewGroups(combinations[i]) } };
                    var result = Battle.FastSim(attackHands, defenceHands, defenceTurrelsGroups);

                    if (result >= bestResult) continue;
                    bestResult = result;
                    bestIndex = i;
                }
                return (bestResult, bestIndex);
            });
            return ret;
        }

        void Combine(List<List<UnitsView>> items, List<UnitsView> addedItems, decimal handWeight, List<List<UnitsView>> combinations, int index)
        {
            foreach (var item in items[index]
                .Where(p => p.Totalweight
                            + addedItems.Sum(pp => pp.Totalweight) <= handWeight).ToList())
            {
                var newItem = new List<UnitsView>(addedItems) { item };
                combinations.Add(newItem);
                for (var i = index + 1; i < items.Count; i++)
                {
                    Combine(items, newItem, handWeight, combinations, i);
                }
            }
        }

    }
}
