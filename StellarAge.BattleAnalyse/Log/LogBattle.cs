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
    }
}
