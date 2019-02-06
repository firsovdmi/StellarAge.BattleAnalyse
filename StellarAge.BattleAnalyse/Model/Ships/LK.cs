using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class LK : Ship
    {
        public override string Name => "Линкор";
        public override long Weight => 600;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(KR),
            typeof(TN),
            typeof(LI),
            typeof(TI),
            typeof(LK),
            typeof(BB),
            typeof(ES),
            typeof(DR),
            typeof(LT),
            typeof(TT),
            typeof(LU),
            typeof(PU),
            typeof(RU),
            typeof(GU),
            typeof(IU),
        };

    }
}