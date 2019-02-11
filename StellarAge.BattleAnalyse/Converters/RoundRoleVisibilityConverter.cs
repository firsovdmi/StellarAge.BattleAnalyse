using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using StellarAge.BattleAnalyse.Log;

namespace StellarAge.BattleAnalyse.Converters
{
    public class RoundRoleVisibilityConverter : ConvertorBase<RoundRoleVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var roundRole = (RoundRole) value;
            if (roundRole == RoundRole.Agressor || roundRole == RoundRole.Defender)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }
    }
}
