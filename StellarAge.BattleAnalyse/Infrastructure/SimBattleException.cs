using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarAge.BattleAnalyse.Infrastructure
{
    public class SimBattleException : Exception
    {
        public SimBattleException(string message) : base(message)
        {

        }
    }
}
