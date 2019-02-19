using System;
using System.Collections.Generic;
using StellarAge.BattleAnalyse.Model.Common;

namespace StellarAge.BattleAnalyse.Model.Ships
{
    class LT : Ship
    {
        public override bool IsBattle => false;
        public override string Name => "Легкий транспорт";
        public override long Weight => 100;
        public override List<Type> TargetPriority => new List<Type>();
    }
}