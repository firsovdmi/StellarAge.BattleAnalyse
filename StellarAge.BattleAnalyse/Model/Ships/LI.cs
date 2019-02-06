using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class LI : Ship
    {
        public override string Name => "Легий истребитель";
        public override long Weight => 20;

        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(KR),
            typeof(BB),
            typeof(TN),
            typeof(LI),
            typeof(LK),
            typeof(ES),
            typeof(TI),
            typeof(DR),
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