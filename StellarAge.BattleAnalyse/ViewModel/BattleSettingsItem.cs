using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StellarAge.BattleAnalyse.ViewModel
{
    [Serializable]
    public class BattleSettingsItem
    {
        public BattleSettingsItem()
        {
            AttackHands = new List<HandView>();
            DefenceHands = new List<HandView>();
            DefenceTurrels = new List<UnitsView>();
        }
        [DataMember] public List<HandView> AttackHands { get; set; }
        [DataMember] public List<HandView> DefenceHands { get; set; }
        [DataMember] public List<UnitsView> DefenceTurrels { get; set; }
        [DataMember] public decimal HandWeight { get; set; }
        [DataMember] public decimal Precision { get; set; }
        [DataMember] public int ThreadCount { get; set; }

        public bool ReadyForSimulation()
        {
            var ret = AttackHands.Any(p => p.UnitsView.Any(pp => pp.Count > 0))
                      && (DefenceHands.Any(p => p.UnitsView.Any(pp => pp.Count > 0))
                || DefenceTurrels.Any(p => p.Count > 0));
            return ret;
        }
    }
}