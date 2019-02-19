using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Ships;

namespace StellarAge.BattleAnalyse.Model.Turrels
{
    class IU : Turrel
    {
        public override bool IsBattle => false;
        public override string Name => "Ионная установка";
        public override long Weight => 500;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(LK),
            typeof(DR),
            typeof(ES),
            typeof(LI),
            typeof(TI),
            typeof(KR),
            typeof(BB),
            typeof(TN),
            typeof(LT),
            typeof(TT),
        };
    }
}