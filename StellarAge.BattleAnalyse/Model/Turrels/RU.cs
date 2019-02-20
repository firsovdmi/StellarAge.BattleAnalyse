using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Ships;

namespace StellarAge.BattleAnalyse.Model.Turrels
{
    class RU : Turrel
    {
        public override bool IsBattle => true;
        public override string Name => "Ракетная установка";
        public override long Weight => 10;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(LI),
            typeof(TI),
            typeof(LK),
            typeof(KR),
            typeof(DR),
            typeof(ES),
            typeof(BB),
            typeof(TN),
            typeof(LT),
            typeof(TT),
        };
    }
}