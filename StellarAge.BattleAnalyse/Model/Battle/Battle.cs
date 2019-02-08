using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using StellarAge.BattleAnalyse.Infrastructure;
using StellarAge.BattleAnalyse.Log;
using StellarAge.BattleAnalyse.Model.Common;

namespace StellarAge.BattleAnalyse.Model.Battle
{
    class Battle
    {
        public List<Hand> AttackHands { get; set; }
        public List<Hand> DefenceHands { get; set; }
        public List<UnitGroup> DefenceTurrelsGroups { get; set; }

        public LogBattle SimBattle()
        {

            var logBattle = new LogBattle();


            while (AttackHands.Any(p => p.AnyAlive)
                   && (AttackHands.Any(p => p.AnyAlive) || DefenceTurrelsGroups.Any(p => p.AliveUnits.Any())))
            {
                // Раунд Атакующего
                var values = SelectAttackUnits(AttackHands);
                var attackHand = values.Hand;
                var attackUnitsGroup = values.Group;
                var defenceHand = DefenceHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault() ??
                                  new Hand {UnitGroups = DefenceTurrelsGroups};
                var logRound = ExecuteRoundWithLog(attackHand, defenceHand, DefenceTurrelsGroups, attackUnitsGroup);
                logBattle.Rounds.Add(logRound);

                // Раунд кораблей Обороняющегося
                values = SelectAttackUnits(DefenceHands);
                attackHand = values.Hand;
                attackUnitsGroup = values.Group;
                defenceHand = AttackHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault();
                if (defenceHand?.AnyAlive == true && attackHand?.AnyAlive == true)
                {
                    logRound = ExecuteRoundWithLog(attackHand, defenceHand, DefenceTurrelsGroups, attackUnitsGroup);
                    logBattle.Rounds.Add(logRound);
                }

                // Раунд пушек Обороняющегося
                values = SelectAttackUnits(DefenceTurrelsGroups);
                attackHand = values.Hand;
                attackUnitsGroup = values.Group;
                defenceHand = AttackHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault();
                if (defenceHand?.AnyAlive == true && attackHand?.AnyAlive == true)
                {
                    var isDefenceTakeDamage = DefenceHands.Any(p => p.AnyAlive);
                    logRound = ExecuteRoundWithLog(attackHand, defenceHand, DefenceTurrelsGroups, attackUnitsGroup, isDefenceTakeDamage);
                    logBattle.Rounds.Add(logRound);
                }
            }
            return logBattle;
        }

        private (Hand Hand, UnitGroup Group) SelectAttackUnits(List<UnitGroup> groups)
        {
            return SelectAttackUnits(new List<Hand> { new Hand { UnitGroups = groups } });
        }
        private (Hand Hand, UnitGroup Group) SelectAttackUnits(List<Hand> hands)
        {
            if (!hands.Any()) return (null, null);
            var liveHands = hands.Where(p => p.AnyAlive).ToList();
            if (!liveHands.Any()) return (null, null);
            if (hands.All(p => p.HasMoved))
            {
                hands.ForEach(p => p.HasMoved = false);
            }
            var hand = hands.Where(p => !p.HasMoved).OrderBy(p => p.AttackOrder).First();
            var group = hand.SelectNextUnitGroup();
            return (hand, group);
        }

        private LogRound ExecuteRoundWithLog(Hand attackHand, Hand defenceHand, List<UnitGroup> defenceTurrelTypes, UnitGroup currentAttackGroup, bool isDefenceTakeDamage = true)
        {
            var logRound = new LogRound
            {
                RoundType = RoundType.AgressorFleet,
                StartAttackFleetGroups = GetLogUnitGroups(attackHand.UnitGroups),
                StartDefenceFleetGroups = GetLogUnitGroups(defenceHand.UnitGroups),
                StartDefenceTurrelGroups = GetLogUnitGroups(defenceTurrelTypes)
            };
            var result = ExecuteRound(currentAttackGroup, defenceHand.UnitGroups, isDefenceTakeDamage);
            logRound.StartAttackFleetGroups = GetLogUnitGroups(attackHand.UnitGroups);
            logRound.StartDefenceFleetGroups = GetLogUnitGroups(defenceHand.UnitGroups);
            logRound.StartDefenceTurrelGroups = GetLogUnitGroups(defenceTurrelTypes);
            var agressor = logRound.StartAttackFleetGroups.FirstOrDefault(p => p.Nmae == result.AttackGroup.AnyUnit.Name);
            if (agressor != null)
            {
                agressor.RoundRole = RoundRole.Agressor;
            }
            var defender = logRound.StartDefenceFleetGroups.FirstOrDefault(p => p.Nmae == result.DefenceGroup.AnyUnit.Name);
            if (defender != null)
            {
                defender.RoundRole = RoundRole.Defender;
            }
            else
            {
                defender = logRound.StartDefenceTurrelGroups.FirstOrDefault(p => p.Nmae == result.DefenceGroup.AnyUnit.Name);
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

        private RoundResult ExecuteRound(UnitGroup attackType, IEnumerable<UnitGroup> defenceGroups, bool isDefenceTakeDamage)
        {
            var currentDefenceType = attackType.SelectTarget(defenceGroups);
            var storredDefenceAttackPower = currentDefenceType.AllAliveUnitsAttackPower;
            if (isDefenceTakeDamage) currentDefenceType.TakeDamage(attackType.AllAliveUnitsAttackPower);
            attackType.TakeDamage(storredDefenceAttackPower);
            return new RoundResult
            {
                AttackGroup = attackType,
                DefenceGroup = currentDefenceType
            };
        }
    }
}