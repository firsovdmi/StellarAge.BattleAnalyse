using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Ships;

namespace StellarAge.BattleAnalyse.Model.Turrels
{
    class GU : Turrel
    {
        public override bool IsBattle => false;
        public override string Name => "Гаусоввая установка";
        public override long Weight => 200;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(KR),
            typeof(LK),
            typeof(BB),
            typeof(LI),
            typeof(TI),
            typeof(DR),
            typeof(ES),
            typeof(TN),
            typeof(LT),
            typeof(TT),
        };

    }
}