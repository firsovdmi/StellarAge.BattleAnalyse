using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

//            var ttt = Battle.FastSim(battle.AttackHands, battle.DefenceHands, battle.DefenceTurrelsGroups);

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
    }
}
