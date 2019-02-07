using System.Collections.Generic;
using System.Linq;

namespace StellarAge.BattleAnalyse.Model.Common
{
    class UnitGroup
    {
        public List<Unit> Units { get; set; }
        public List<Unit> AliveUnits => Units.Where(p => p.UnitStatus == UnitStatus.Alive).ToList();
        public long Weight => AnyUnit.Weight;

        public Unit AnyUnit => Units.First();
        public long AllAliveUnitsAttackPower => AliveUnits.Count * AnyUnit.AttackPower;
        public long UnitArmor => AnyUnit.NominalArmor;
        public List<Unit> LossUnits => Units.Where(p => p.UnitStatus == UnitStatus.Destroyed).ToList();

        public UnitGroup SelectTarget(IEnumerable<UnitGroup> targets)
        {
            if (targets == null) return null;
            if (!(AnyUnit is Ship anyShip)) return null;
            var unitGroups = targets.ToList();
            foreach (var targetType in anyShip.TargetPriority)
            {
                UnitGroup target = null;
                foreach (var p in unitGroups)
                {
                    if (p.AnyUnit.GetType() != targetType) continue;
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
            // сначала атакуем раненый юнит. Если удалось его добить, едем дальше...
            var damagetUnits = AliveUnits.Where(p => p.CurrentArmor < p.NominalArmor).ToList();
            foreach (var damagetUnit in damagetUnits)
            {
                if (damagetUnit.CurrentArmor > attackPower)
                {
                    damagetUnit.CurrentArmor -= attackPower;
                    return;
                }
                attackPower -= damagetUnit.CurrentArmor;
                damagetUnit.Destroy();
            }

            // вычисляем, сколько юнитов уничтожено
            var completelyDestroyedUnitCount = (int)(attackPower / UnitArmor);
            var attackPowerForLastUnit = completelyDestroyedUnitCount == 0 ? attackPower : attackPower % completelyDestroyedUnitCount;
            foreach (var unit in AliveUnits.Take(completelyDestroyedUnitCount).ToList())
            {
                unit.Destroy();
            }

            // докладываем остатки урона в любой юнит
            if (!AliveUnits.Any()) return;
            AliveUnits.First().CurrentArmor -= attackPowerForLastUnit;
        }

        public void ResetArmor()
        {
            Units.ForEach(p => p.ResetArmor());
        }
    }
}