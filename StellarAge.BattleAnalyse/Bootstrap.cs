using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.ViewModel;

namespace StellarAge.BattleAnalyse
{
    class Bootstrap
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UnitsView, UnitsView>();
                cfg.CreateMap<UnitsView, Unit>();
            });
        }
    }
}
