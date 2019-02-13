using System.Collections.Generic;
using Xceed.Wpf.Toolkit;

namespace StellarAge.BattleAnalyse.Log
{
    public class LogRound
    {
        public RoundType RoundType { get; set; }
        public List<LogUnitGroup> StartAttackFleetGroups { get; set; }
        public List<LogUnitGroup> StartDefenceFleetGroups { get; set; }
        public List<LogUnitGroup> EndAttackFleetGroups { get; set; }
        public List<LogUnitGroup> EndDefenceFleetGroups { get; set; }
        public LogShipFight StartAttackFleetDetail { get; set; }
        public LogShipFight StartDefenceFleetDetail { get; set; }
        public LogShipFight EndAttackFleetDetail { get; set; }
        public LogShipFight EndDefenceFleetDetail { get; set; }
    }
}