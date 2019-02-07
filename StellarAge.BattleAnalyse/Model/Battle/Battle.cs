using System.Collections.Generic;
using System.Linq;
using StellarAge.BattleAnalyse.Infrastructure;
using StellarAge.BattleAnalyse.Log;
using StellarAge.BattleAnalyse.Model.Common;

namespace StellarAge.BattleAnalyse.Model.Battle
{
    class Battle
    {
        public List<Unit> AttackFleet { get; set; }
        public List<Unit> DefenceFleet { get; set; }
        public List<Unit> DefenceTurrels { get; set; }

        public LogBattle SimBattle()
        {
            if (AttackFleet.Any(p => p.AttackPower == 0 || p.NominalArmor == 0))
            {
                throw new SimBattleException("Не задана атака или броня");
            }
            var attackTypes = GetUnitGroups(AttackFleet);
            var defenceShipTypes = GetUnitGroups(DefenceFleet);
            var defenceTurrelTypes = GetUnitGroups(DefenceTurrels);

            attackTypes.ResetArmor();
            defenceShipTypes.ResetArmor();
            defenceTurrelTypes.ResetArmor();
            var logBattle = new LogBattle();
            while (attackTypes.AnyAlive && (defenceShipTypes.AnyAlive || defenceTurrelTypes.AnyAlive))
            {
                var a = attackTypes.Items.Sum(p => p.AliveUnits.Count);
                var d = defenceShipTypes.Items.Sum(p => p.AliveUnits.Count);
                // Раунд Атакующего
                var logRound = ExecuteRoundWithLog(attackTypes.Items, defenceShipTypes.Items, defenceTurrelTypes.Items, attackTypes.Items, defenceShipTypes.Items.Concat(defenceTurrelTypes.Items).ToList());
                logBattle.Rounds.Add(logRound);

                // Раунд кораблей Обороняющегося
                if (defenceShipTypes.AnyAlive && attackTypes.AnyAlive)
                {
                    logRound = ExecuteRoundWithLog(attackTypes.Items, defenceShipTypes.Items, defenceTurrelTypes.Items, defenceShipTypes.Items, attackTypes.Items);
                    logBattle.Rounds.Add(logRound);
                }

                // Раунд пушек Обороняющегося
                if (defenceTurrelTypes.AnyAlive && attackTypes.AnyAlive)
                {
                    logRound = ExecuteRoundWithLog(attackTypes.Items, defenceShipTypes.Items, defenceTurrelTypes.Items, defenceTurrelTypes.Items, attackTypes.Items, defenceShipTypes.AnyAlive);
                    logBattle.Rounds.Add(logRound);
                }
            }
            return logBattle;
        }

        private LogRound ExecuteRoundWithLog(List<UnitGroup> attackTypes, List<UnitGroup> defenceShipTypes, List<UnitGroup> defenceTurrelTypes, List<UnitGroup> currentAttackTypes, List<UnitGroup> currentDefenceShipTypes, bool isDefenceTakeDamage = true)
        {
            var logRound = new LogRound
            {
                RoundType = RoundType.AgressorFleet,
                StartAttackFleetGroups = GetLogUnitGroups(attackTypes),
                StartDefenceFleetGroups = GetLogUnitGroups(defenceShipTypes),
                StartDefenceTurrelGroups = GetLogUnitGroups(defenceTurrelTypes)
            };
            var result = ExecuteRound(currentAttackTypes, currentDefenceShipTypes, isDefenceTakeDamage);
            logRound.EndAttackFleetGroups = GetLogUnitGroups(attackTypes);
            logRound.EndDefenceFleetGroups = GetLogUnitGroups(defenceShipTypes);
            logRound.EndDefenceTurrelGroups = GetLogUnitGroups(defenceTurrelTypes);
            var agressor = logRound.StartAttackFleetGroups.FirstOrDefault(p => p.Nmae == result.AttackType.AnyUnit.Name);
            if (agressor != null)
            {
                agressor.RoundRole = RoundRole.Agressor;
            }
            var defender = logRound.StartDefenceFleetGroups.FirstOrDefault(p => p.Nmae == result.DefenceType.AnyUnit.Name);
            if (defender != null)
            {
                defender.RoundRole = RoundRole.Defender;
            }
            else
            {
                defender = logRound.StartDefenceTurrelGroups.FirstOrDefault(p => p.Nmae == result.DefenceType.AnyUnit.Name);
                if (defender != null)
                {
                    defender.RoundRole = RoundRole.Defender;
                }
            }
            return logRound;
        }

        private List<LogUnitGroup> GetLogUnitGroups(List<UnitGroup> groups)
        {
            var ret = groups.Select(p => new LogUnitGroup
            {
                Count = p.Units.Count,
                Nmae = p.AnyUnit.Name,
                TotalArmor = p.UnitArmor,
                TotalAttack = p.AllAliveUnitsAttackPower
            }).ToList();
            return ret;
        }

        private RoundResult ExecuteRound(IEnumerable<UnitGroup> attackTypes, IEnumerable<UnitGroup> defenceTypes, bool isDefenceTakeDamage)
        {
            var currentAttackType = attackTypes.OrderBy(p => p.Weight).FirstOrDefault();
            if (currentAttackType == null)
            {
                return null;
            }
            var currentDefenceType = currentAttackType.SelectTarget(defenceTypes);
            var storredDefenceAttackPower = currentDefenceType.AllAliveUnitsAttackPower;
            if (isDefenceTakeDamage) currentDefenceType.TakeDamage(currentAttackType.AllAliveUnitsAttackPower);
            currentAttackType.TakeDamage(storredDefenceAttackPower);
            return new RoundResult
            {
                AttackType = currentAttackType,
                DefenceType = currentDefenceType
            };
        }

        private UnitGroups GetUnitGroups(IEnumerable<Unit> items)
        {
            var ret = new UnitGroups(
                items.GroupBy(p => p.GetType()).Select(p => new UnitGroup
                {
                    Units = p.ToList()
                }).ToList());
            return ret;
        }

        public class UnitGroups
        {
            public UnitGroups(List<UnitGroup> items = null)
            {
                Items = items ?? new List<UnitGroup>();
            }
            public List<UnitGroup> Items { get; set; }
            public bool AnyAlive => Items.Any(p => p.AliveUnits.Count > 0);

            public void ResetArmor()
            {
                Items.ForEach(p => p.ResetArmor());
            }
        }
    }
}