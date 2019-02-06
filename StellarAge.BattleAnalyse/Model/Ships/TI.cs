using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class TI : Ship
    {
        public override string Name => "Тяжелый истребитель";
        public override long Weight => 50;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(LI),
            typeof(DR),
            typeof(BB),
            typeof(TI),
            typeof(LK),
            typeof(TN),
            typeof(KR),
            typeof(ES),
            typeof(LT),
            typeof(TT),
            typeof(GU),
            typeof(IU),
            typeof(PU),
            typeof(RU),
            typeof(LU),
        };
    }
}