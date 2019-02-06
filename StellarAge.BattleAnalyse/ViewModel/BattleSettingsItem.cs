using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StellarAge.BattleAnalyse.ViewModel
{
    [Serializable]
    public class BattleSettingsItem
    {
        [DataMember] public List<UnitsView> AttackUnits { get; set; }
        [DataMember] public List<UnitsView> DefenceUnits { get; set; }
        [DataMember] public List<UnitsView> DefenceTurrels { get; set; }

        public bool ReadyForSimulation()
        {
            var ret = AttackUnits.All(p => p.Count > 0) && DefenceUnits.All(p => p.Count > 0);
            return ret;
        }
    }
}