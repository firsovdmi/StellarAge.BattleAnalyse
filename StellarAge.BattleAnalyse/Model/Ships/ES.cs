using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class ES : Ship
    {
        public override bool IsBattle => true;
        public override string Name => "Эсминец";
        public override long Weight => 5000;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(TI),
            typeof(DR),
            typeof(LI),
            typeof(LK),
            typeof(BB),
            typeof(ES),
            typeof(KR),
            typeof(TN),
            typeof(LT),
            typeof(TT),
            typeof(RU),
            typeof(LU),
            typeof(GU),
            typeof(IU),
            typeof(PU),
        };
    }
}