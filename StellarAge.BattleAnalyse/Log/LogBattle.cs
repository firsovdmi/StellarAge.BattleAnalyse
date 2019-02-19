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
        public long StartWeight { get; set; }
        public long EndWeight { get; set; }

        public decimal LossPercenatge
        {
            get
            {
                if (StartWeight == 0) return 0;
                var ret =100-  EndWeight * 100 / StartWeight;
                return ret;
            }
        }
    }
}
