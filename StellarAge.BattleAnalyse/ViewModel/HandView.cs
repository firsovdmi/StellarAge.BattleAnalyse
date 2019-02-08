using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StellarAge.BattleAnalyse.ViewModel
{
    [Serializable]
    public class HandView
    {
        [DataMember]
        public List<UnitsView> UnitsView { get; set; }
    }
}