using System;
using System.Globalization;
using System.Windows;

namespace StellarAge.BattleAnalyse.Converters
{
    class BoolVisibleConverter : ConvertorBase<BoolVisibleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}