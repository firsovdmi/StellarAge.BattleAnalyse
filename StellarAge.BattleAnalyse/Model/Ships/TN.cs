using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Turrels;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class TN : Ship
    {
        public override bool IsBattle => true;
        public override string Name => "Титан";
        public override long Weight => 20000;
        public override List<Type> TargetPriority => new List<Type>
        {
            typeof(DR),
            typeof(ES),
            typeof(RU),
            typeof(LU),
            typeof(GU),
            typeof(IU),
            typeof(PU),
            typeof(TI),
            typeof(KR),
            typeof(BB),
            typeof(TN),
            typeof(LI),
            typeof(LK),
            typeof(LT),
            typeof(TT),
        };

    }
}