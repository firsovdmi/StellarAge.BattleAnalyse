using System;
using System.Collections.Generic;
using System.Linq;
using StellarAge.BattleAnalyse.Model.Ships;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Common
{
    abstract class Unit
    {
        public abstract string Name { get; }
        public abstract long Weight { get; }
        public long NominalUnitArmor { get; set; }
        public long CurrentArmor { get; set; }
        public long AttackPower { get; set; }
        public long TotalAttackPower => AttackPower * UnitsCount;
        public abstract List<Type> TargetPriority { get; }
        public UnitStatus UnitStatus { get; set; }
        public bool HasMoved { get; set; }
        public long NominalUnitsCount { get; set; }
        public long UnitsCount { get; set; }
        public bool IsAnyAlive => UnitsCount > 0;
        public long TotalWeight => UnitsCount * Weight;
        public abstract bool IsBattle { get; }

        public void ResetArmor()
        {
            UnitsCount = NominalUnitsCount;
            CurrentArmor = NominalUnitArmor * UnitsCount;
        }

        public void Destroy()
        {
            CurrentArmor = 0;
            UnitStatus = UnitStatus.Destroyed;
        }
        public Unit SelectTarget(IEnumerable<Unit> targets)
        {
            if (targets == null) return null;
            // if (!(this is Ship anyShip)) return null;
            var unitGroups = targets.Where(p => p.IsAnyAlive).ToList();
            foreach (var targetType in TargetPriority)
            {
                Unit target = null;
                foreach (var p in unitGroups)
                {
                    if (p.GetType() != targetType) continue;
                    target = p;
                    break;
                }

                if (target != null)
                {
                    return target;
                }
            }
            return null;
        }

        public void TakeDamage(long attackPower)
        {
            CurrentArmor -= attackPower;
            var damagedUnitArmor = CurrentArmor % NominalUnitArmor;
            UnitsCount = CurrentArmor / NominalUnitArmor;
            if (damagedUnitArmor > 0) UnitsCount++;
            CurrentArmor = UnitsCount * NominalUnitArmor;
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] AliveUnits = {UnitsCount}";
        }

        public void ResetMoved()
        {
            HasMoved = false;
        }
    }
}