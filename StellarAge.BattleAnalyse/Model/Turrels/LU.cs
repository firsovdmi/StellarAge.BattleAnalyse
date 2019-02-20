using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Ships;

namespace StellarAge.BattleAnalyse.Model.Turrels
{
    class LU : Turrel
    {
        public override bool IsBattle => true;
        public override string Name => "Лазерная установка";
        public override long Weight => 40;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(LI),
            typeof(TI),
            typeof(KR),
            typeof(DR),
            typeof(LK),
            typeof(ES),
            typeof(BB),
            typeof(TN),
            typeof(LT),
            typeof(TT),
        };

    }
}