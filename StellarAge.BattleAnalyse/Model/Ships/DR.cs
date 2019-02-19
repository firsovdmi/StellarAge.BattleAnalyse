using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class DR : Ship
    {
        public override bool IsBattle => true;
        public override string Name => "Дредноут";
        public override long Weight => 1500;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(LI),
            typeof(LK),
            typeof(KR),
            typeof(DR),
            typeof(BB),
            typeof(TI),
            typeof(ES),
            typeof(TN),
            typeof(LT),
            typeof(TT),
            typeof(RU),
            typeof(GU),
            typeof(PU),
            typeof(LU),
            typeof(IU),
        };
    }
}