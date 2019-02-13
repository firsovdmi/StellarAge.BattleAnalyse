using System.Collections.Generic;
using System.Linq;

namespace StellarAge.BattleAnalyse.Model.Common
{
    class Hand
    {
        public int AttackOrder { get; set; }
        public List<Unit> UnitGroups { get; set; }

        public bool HasMoved
        {
            get { return UnitGroups.Where(p=>p.IsAnyAlive).All(p => p.HasMoved); }
        }
    
        public bool AnyAlive => UnitGroups.Any(p => p.IsAnyAlive);

        public void ResetMoved()
        {
            UnitGroups.ForEach(p=>p.ResetMoved());
        }
        public Unit SelectNextUnitGroup()
        {
            var ret = UnitGroups
                .Where(p => !p.HasMoved)
                .Where(p => p.IsAnyAlive)
                .OrderBy(p => p.Weight)
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