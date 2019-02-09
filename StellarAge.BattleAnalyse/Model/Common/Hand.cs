using System.Collections.Generic;
using System.Linq;

namespace StellarAge.BattleAnalyse.Model.Common
{
    class Hand
    {
        public int AttackOrder { get; set; }
        public List<UnitGroup> UnitGroups { get; set; }

        public bool HasMoved
        {
            get { return UnitGroups.Where(p=>p.IsAnyUnitAlive).All(p => p.HasMoved); }
        }
    
        public bool AnyAlive => UnitGroups.Any(p => p.AliveUnits.Any());
        public IEnumerable<Unit> AliveUnis => UnitGroups.SelectMany(p => p.AliveUnits);

        public void ResetMoved()
        {
            UnitGroups.ForEach(p=>p.ResetMoved());
        }
        public UnitGroup SelectNextUnitGroup()
        {
            var ret = UnitGroups
                .Where(p => !p.HasMoved)
                .Where(p => p.IsAnyUnitAlive)
                .OrderBy(p => p.AnyUnit.Weight)
                .FirstOrDefault();
            if (ret != null) ret.HasMoved = true;
            return ret;
        }

        public void ResetArmor()
        {
            UnitGroups.ForEach(p => p.ResetArmor());
        }
    }
}