using System.Collections.Generic;

namespace StellarAge.BattleAnalyse.Log
{
    public class LogBattle
    {
        public LogBattle()
        {
            Rounds = new List<LogRound>();
        }
        public List<LogRound> Rounds { get; set; }
        public long StartAttackUnitCount { get; set; }
        public long EndAttackUnitCount { get; set; }

        public decimal LossPercenatge
        {
            get
            {
                if (StartAttackUnitCount == 0) return 0;
                var ret = (StartAttackUnitCount - EndAttackUnitCount) * 100 / StartAttackUnitCount;
                return ret;
            }
        }
    }
}
