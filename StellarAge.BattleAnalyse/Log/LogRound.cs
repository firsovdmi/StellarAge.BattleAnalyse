using System.Collections.Generic;

namespace StellarAge.BattleAnalyse.Log
{
    public class LogRound
    {
        public RoundType RoundType { get; set; }
        public List<LogUnitGroup> StartAttackFleetGroups { get; set; }
        public List<LogUnitGroup> StartDefenceFleetGroups { get; set; }
        public List<LogUnitGroup> StartDefenceTurrelGroups { get; set; }
        public List<LogUnitGroup> EndAttackFleetGroups { get; set; }
        public List<LogUnitGroup> EndDefenceFleetGroups { get; set; }
        public List<LogUnitGroup> EndDefenceTurrelGroups { get; set; }
    }
}