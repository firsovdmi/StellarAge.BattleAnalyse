using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class BB : Ship
    {
        public override string Name => "Бомбордировщик";
        public override long Weight => 2000;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(RU),
            typeof(LU),
            typeof(GU),
            typeof(IU),
            typeof(PU),
            typeof(KR),
            typeof(LK),
            typeof(DR),
            typeof(BB),
            typeof(ES),
            typeof(TN),
            typeof(LI),
            typeof(TI),
            typeof(LT),
            typeof(TT),
        };

    }
}