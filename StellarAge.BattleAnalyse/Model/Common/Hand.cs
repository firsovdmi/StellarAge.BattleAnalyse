using System.Collections.Generic;
using System.Linq;

namespace StellarAge.BattleAnalyse.Model.Common
{
    class Hand
    {
        public int AttackOrder { get; set; }
        public List<UnitGroup> UnitGroups { get; set; }
        public bool HasMoved { get; set; }
        public bool AnyAlive => UnitGroups.Any(p => p.AliveUnits.Any());

        public UnitGroup SelectNextUnitGroup()
        {
            var ret = UnitGroups
                .Where(p => !p.HasMoved)
                .Where(p => p.Units.Any(pp => pp.UnitStatus == UnitStatus.Alive))
                .OrderBy(p => p.AnyUnit.Weight)
                .FirstOrDefault();
            if (ret != null) ret.HasMoved = true;
            return ret;
        }
    }
}