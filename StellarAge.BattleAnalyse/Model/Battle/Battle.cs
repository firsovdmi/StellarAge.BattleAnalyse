using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using StellarAge.BattleAnalyse.Infrastructure;
using StellarAge.BattleAnalyse.Log;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Ships;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Battle
{
    class Battle
    {
        public List<Hand> AttackHands { get; set; }
        public List<Hand> DefenceHands { get; set; }
        public List<Unit> DefenceTurrelsGroups { get; set; }

        public LogBattle SimBattle()
        {

            var logBattle = new LogBattle();
            AttackHands.ForEach(p => p.ResetArmor());
            DefenceHands.ForEach(p => p.ResetArmor());
            DefenceTurrelsGroups.ForEach(p => p.ResetArmor());
            logBattle.StartAttackUnitCount = AttackHands.Sum(p => p.UnitGroups.Sum(pp => pp.UnitsCount));
            while (AttackHands.Any(p => p.AnyAlive)
                   && (DefenceHands.Any(p => p.AnyAlive) || DefenceTurrelsGroups.Any(p => p.IsAnyAlive)))
            {
                // Раунд Атакующего
                var values = SelectAttackUnits(AttackHands);
                var attackHand = values.Hand;
                var attackUnitsGroup = values.Group;
                var defenceHand = DefenceHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault() ??
                                  new Hand { UnitGroups = DefenceTurrelsGroups };
                var logRound = ExecuteRoundWithLog(attackUnitsGroup, defenceHand.UnitGroups, true, attackHand.UnitGroups, defenceHand.UnitGroups.Concat( DefenceTurrelsGroups).ToList());
                logRound.RoundType = RoundType.AgressorFleet;
                logBattle.Rounds.Add(logRound);

                // Раунд кораблей Обороняющегося
                values = SelectAttackUnits(DefenceHands);
                attackHand = values.Hand;
                attackUnitsGroup = values.Group;
                defenceHand = AttackHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault();
                if (defenceHand?.AnyAlive == true && attackHand?.AnyAlive == true)
                {
                    logRound = ExecuteRoundWithLog(attackUnitsGroup, defenceHand.UnitGroups, true, attackHand.UnitGroups.Concat(DefenceTurrelsGroups).ToList(), defenceHand.UnitGroups);
                    logRound.RoundType = RoundType.DefenceFleet;
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
                    logRound = ExecuteRoundWithLog(attackUnitsGroup, defenceHand.UnitGroups, isDefenceTakeDamage, attackHand.UnitGroups, defenceHand.UnitGroups);
                    logRound.RoundType = RoundType.DefenceTurrels;
                    logBattle.Rounds.Add(logRound);
                }
            }
            logBattle.EndAttackUnitCount = AttackHands.Sum(p => p.UnitGroups.Sum(pp => pp.UnitsCount));
            return logBattle;
        }

        public static decimal FastSim(List<Hand> attackHands, List<Hand> defenceHands, List<Unit> defenceTurrelsGroups)
        {
            var startCount = attackHands.Sum(p => p.UnitGroups.Sum(pp => pp.UnitsCount));
            if (startCount == 0) return 0;
            attackHands.ForEach(p => p.ResetArmor());
            defenceHands.ForEach(p => p.ResetArmor());
            defenceTurrelsGroups.ForEach(p => p.ResetArmor());
            while (attackHands.Any(p => p.AnyAlive)
                   && (defenceHands.Any(p => p.AnyAlive) || defenceTurrelsGroups.Any(p => p.IsAnyAlive)))
            {
                // Раунд Атакующего
                var values = SelectAttackUnits(attackHands);
                var attackUnitsGroup = values.Group;
                var defenceHand = defenceHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault() ??
                                  new Hand { UnitGroups = defenceTurrelsGroups };
                ExecuteRound(attackUnitsGroup, defenceHand.UnitGroups, true);


                // Раунд кораблей Обороняющегося
                values = SelectAttackUnits(defenceHands);
                attackUnitsGroup = values.Group;
                defenceHand = attackHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault();
                if (defenceHand?.AnyAlive == true && attackUnitsGroup?.IsAnyAlive == true)
                {
                    ExecuteRound(attackUnitsGroup, defenceHand.UnitGroups, true);
                }

                // Раунд пушек Обороняющегося
                values = SelectAttackUnits(defenceTurrelsGroups);
                attackUnitsGroup = values.Group;
                defenceHand = attackHands.Where(p => p.AnyAlive).OrderBy(p => p.AttackOrder).FirstOrDefault();
                if (defenceHand?.AnyAlive == true && attackUnitsGroup?.IsAnyAlive == true)
                {
                    var isDefenceTakeDamage = defenceHands.Any(p => p.AnyAlive);
                    ExecuteRound(attackUnitsGroup, defenceHand.UnitGroups, isDefenceTakeDamage);

                }
            }
            var endCount = attackHands.Sum(p => p.UnitGroups.Sum(pp => pp.UnitsCount));
            var ret = 100 - endCount * 100 / startCount;
            return ret;

        }

        private static (Hand Hand, Unit Group) SelectAttackUnits(List<Unit> groups)
        {
            return SelectAttackUnits(new List<Hand> { new Hand { UnitGroups = groups } });
        }
        private static (Hand Hand, Unit Group) SelectAttackUnits(List<Hand> hands)
        {
            if (!hands.Any()) return (null, null);
            var liveHands = hands.Where(p => p.AnyAlive).ToList();
            if (!liveHands.Any()) return (null, null);
            if (hands.All(p => p.HasMoved))
            {
                hands.ForEach(p => p.ResetMoved());
            }
            var hand = hands.Where(p => !p.HasMoved).OrderBy(p => p.AttackOrder).First();
            var group = hand.SelectNextUnitGroup();
            return (hand, group);
        }

        private LogRound ExecuteRoundWithLog(Unit currentAttackUnit, List<Unit> defenceUnits, bool isDefenceTakeDamage, List<Unit> attackUnitsForLog, List<Unit> defenceUnitsForLog)
        {
            var logRound = new LogRound
            {
                RoundType = RoundType.AgressorFleet,
                StartAttackFleetGroups = GetLogUnitGroups(attackUnitsForLog),
                StartDefenceFleetGroups = GetLogUnitGroups(defenceUnitsForLog)
            };

            var result = ExecuteRound(currentAttackUnit, defenceUnits, isDefenceTakeDamage);
            logRound.EndAttackFleetGroups = GetLogUnitGroups(attackUnitsForLog);
            logRound.EndDefenceFleetGroups = GetLogUnitGroups(defenceUnitsForLog);
            var agressor = logRound.StartAttackFleetGroups.FirstOrDefault(p => p.Nmae == result.AttackGroup.Name);
            if (agressor != null)
            {
                agressor.RoundRole = RoundRole.Agressor;
                logRound.StartAttackFleetDetail = GetLogShipFight(agressor, currentAttackUnit);
                var agressorEnd = logRound.EndAttackFleetGroups.FirstOrDefault(p => p.Nmae == result.AttackGroup.Name);
                logRound.EndAttackFleetDetail = GetLogShipFight(agressorEnd, currentAttackUnit, agressor.Count - agressorEnd?.Count ?? 0);
                logRound.StartDefenceFleetGroups.ForEach(p =>
                {
                    var attackBonusCoefficient = GetAttackBonusCoefficients(agressor.ClassName, p.ClassName);
                    if (attackBonusCoefficient > 1)
                    {
                        p.AttackBonusCoefficients = (int)(attackBonusCoefficient * 100);
                    }
                });
            }
            var defender = logRound.StartDefenceFleetGroups.FirstOrDefault(p => p.Nmae == result.DefenceGroup.Name);
            if (defender != null)
            {
                var defendGroup =
                    defenceUnits.FirstOrDefault(p => p.Name == result.DefenceGroup.Name);
                logRound.StartDefenceFleetDetail = GetLogShipFight(defender, defendGroup);
                defender.RoundRole = RoundRole.Defender;
                var endDefender = logRound.EndDefenceFleetGroups.FirstOrDefault(p => p.Nmae == result.DefenceGroup.Name);
                logRound.EndDefenceFleetDetail = GetLogShipFight(endDefender, defendGroup, defender.Count - endDefender?.Count ?? 0);
                logRound.StartAttackFleetGroups.ForEach(p =>
                {
                    var attackBonusCoefficient = GetAttackBonusCoefficients(defender.ClassName, p.ClassName);
                    if (attackBonusCoefficient > 1)
                    {
                        p.AttackBonusCoefficients = (int)(attackBonusCoefficient * 100);
                    }
                });
            }
            return logRound;
        }

        private LogShipFight GetLogShipFight(LogUnitGroup agressor, Unit currentAttackGroup, long destroyedCount = 0)
        {
            var ret = new LogShipFight
            {
                ClassName = agressor.ClassName,
                Count = agressor.Count,
                Name = agressor.Nmae,
                SingleUnitArmor = currentAttackGroup.NominalUnitArmor,
                SingleUnitAtackPower = currentAttackGroup.AttackPower,
                DestroyedCount = destroyedCount
            };
            return ret;
        }


        private List<LogUnitGroup> GetLogUnitGroups(List<Unit> groups)
        {
            var ret = groups.Select(p => new LogUnitGroup
            {
                Count = p.UnitsCount,
                Nmae = p.Name,
                ClassName = p.GetType().Name,
                TotalArmor = p.CurrentArmor,
                TotalAttack = p.AttackPower
            }).ToList();
            return ret;
        }

        private static RoundResult ExecuteRound(Unit attackType, IEnumerable<Unit> defenceGroups, bool isDefenceTakeDamage)
        {
            var currentDefenceType = attackType.SelectTarget(defenceGroups);
            var storredDefenceAttackPower = currentDefenceType.TotalAttackPower;
            var attackBonusCoefficient =
                GetAttackBonusCoefficients(attackType.GetType().Name, currentDefenceType.GetType().Name);
            var defenceBonusCoefficient =
                GetAttackBonusCoefficients(currentDefenceType.GetType().Name, attackType.GetType().Name);
            if (isDefenceTakeDamage) currentDefenceType.TakeDamage((long)(attackType.TotalAttackPower * attackBonusCoefficient));
            attackType.TakeDamage((long)(storredDefenceAttackPower * defenceBonusCoefficient));
            return new RoundResult
            {
                AttackGroup = attackType,
                DefenceGroup = currentDefenceType
            };
        }
        public static decimal GetAttackBonusCoefficients(string attackUnitName, string defenceUnitName)
        {
            var key = new Tuple<string, string>(attackUnitName, defenceUnitName);
            return AttackBonusCoefficients.ContainsKey(key) ? AttackBonusCoefficients[key] : 1;
        }

        public static Dictionary<Tuple<string, string>, decimal> AttackBonusCoefficients = new Dictionary<Tuple<string, string>, decimal>
        {
            { new Tuple<string, string>(nameof(LI),nameof(KR)),  3},
            { new Tuple<string, string>(nameof(LI),nameof(BB)),  4},
            { new Tuple<string, string>(nameof(LI),nameof(TN)),  3},
            { new Tuple<string, string>(nameof(TI),nameof(LI)),  3},
            { new Tuple<string, string>(nameof(TI),nameof(DR)),  4},
            { new Tuple<string, string>(nameof(TI),nameof(BB)),  3},
            { new Tuple<string, string>(nameof(KR),nameof(TI)),  3},
            { new Tuple<string, string>(nameof(KR),nameof(ES)),  5},
            { new Tuple<string, string>(nameof(LK),nameof(KR)),  5},
            { new Tuple<string, string>(nameof(LK),nameof(TN)),  3},
            { new Tuple<string, string>(nameof(DR),nameof(LI)),  5},
            { new Tuple<string, string>(nameof(DR),nameof(LK)),  5},
            { new Tuple<string, string>(nameof(BB),nameof(RU)),  4},
            { new Tuple<string, string>(nameof(BB),nameof(LU)),  4},
            { new Tuple<string, string>(nameof(BB),nameof(GU)),  4},
            { new Tuple<string, string>(nameof(BB),nameof(IU)),  4},
            { new Tuple<string, string>(nameof(BB),nameof(PU)),  4},
            { new Tuple<string, string>(nameof(ES),nameof(TI)),  5},
            { new Tuple<string, string>(nameof(ES),nameof(DR)),  4},
            { new Tuple<string, string>(nameof(TN),nameof(DR)),  3},
            { new Tuple<string, string>(nameof(TN),nameof(RU)),  5},
            { new Tuple<string, string>(nameof(TN),nameof(LU)),  2},
            { new Tuple<string, string>(nameof(TN),nameof(GU)),  2},
            { new Tuple<string, string>(nameof(TN),nameof(IU)),  2},
            { new Tuple<string, string>(nameof(TN),nameof(KR)),  2},
            { new Tuple<string, string>(nameof(RU),nameof(LI)),  4},
            { new Tuple<string, string>(nameof(RU),nameof(TI)),  4},
            { new Tuple<string, string>(nameof(RU),nameof(LK)),  3},
            { new Tuple<string, string>(nameof(LU),nameof(LI)),  2},
            { new Tuple<string, string>(nameof(LU),nameof(TI)),  2},
            { new Tuple<string, string>(nameof(LU),nameof(KR)),  2},
            { new Tuple<string, string>(nameof(LU),nameof(DR)),  4},
            { new Tuple<string, string>(nameof(GU),nameof(KR)),  4},
            { new Tuple<string, string>(nameof(GU),nameof(LK)),  3},
            { new Tuple<string, string>(nameof(GU),nameof(BB)),  3},
            { new Tuple<string, string>(nameof(IU),nameof(LK)),  3},
            { new Tuple<string, string>(nameof(IU),nameof(DR)),  3},
            { new Tuple<string, string>(nameof(IU),nameof(ES)),  3},
            { new Tuple<string, string>(nameof(PU),nameof(BB)),  3},
            { new Tuple<string, string>(nameof(PU),nameof(ES)),  4},
            { new Tuple<string, string>(nameof(PU),nameof(TN)),  4},
        };
    }
}