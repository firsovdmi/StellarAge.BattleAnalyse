using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class KR : Ship
    {
        public override string Name => "Крейсер";
        public override long Weight => 200;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(TI),
            typeof(ES),
            typeof(KR),
            typeof(DR),
            typeof(BB),
            typeof(TN),
            typeof(LI),
            typeof(LK),
            typeof(LT),
            typeof(TT),
            typeof(RU),
            typeof(IU),
            typeof(PU),
            typeof(LU),
            typeof(GU),
        };

    }
}