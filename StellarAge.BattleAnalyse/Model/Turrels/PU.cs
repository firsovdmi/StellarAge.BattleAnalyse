using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Ships;

namespace StellarAge.BattleAnalyse.Model.Turrels
{
    class PU : Turrel
    {
        public override bool IsBattle => false;
        public override string Name => "Плазменная установка";
        public override long Weight => 1000;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(BB),
            typeof(ES),
            typeof(TN),
            typeof(LI),
            typeof(TI),
            typeof(KR),
            typeof(LK),
            typeof(DR),
            typeof(LT),
            typeof(TT),
        };

    }
}