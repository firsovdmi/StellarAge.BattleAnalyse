using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class TT : Ship
    {
        public override string Name => "Тяжелый транспорт";
        public override long Weight => 1000;
        public override List<Type> TargetPriority => new List<Type>();
    }
}