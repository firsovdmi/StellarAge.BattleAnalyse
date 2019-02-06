using System;
using System.Collections.Generic;

namespace StellarAge.BattleAnalyse.Model.Common
{
    abstract class Unit
    {
        public abstract string Name { get; }
        public abstract long Weight { get; }
        public long NominalArmor { get; set; }
        public long CurrentArmor { get; set; }
        public long AttackPower { get; set; }
        public abstract List<Type> TargetPriority { get; }
        public UnitStatus UnitStatus { get; set; }

        public void ResetArmor()
        {
            CurrentArmor = NominalArmor;
        }

        public void Destroy()
        {
            CurrentArmor = 0;
            UnitStatus = UnitStatus.Destroyed;
        }
    }
}