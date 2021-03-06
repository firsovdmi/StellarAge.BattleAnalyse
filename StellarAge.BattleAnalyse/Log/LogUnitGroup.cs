﻿namespace StellarAge.BattleAnalyse.Log
{
    public class LogUnitGroup
    {
        public string Nmae { get; set; }
        public long Count  { get; set; }
        public long TotalArmor { get; set; }
        public long TotalAttack { get; set; }
        public RoundRole RoundRole { get; set; }
        public string ClassName { get; internal set; }
        public int AttackBonusCoefficients { get; set; }
        public bool IsHasAttackBonusCoefficients => AttackBonusCoefficients > 0;
    }
}