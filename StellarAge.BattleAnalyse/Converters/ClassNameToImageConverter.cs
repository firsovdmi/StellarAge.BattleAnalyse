using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using StellarAge.BattleAnalyse.ViewModel;

namespace StellarAge.BattleAnalyse.Converters
{
    public class ClassNameToImageConverter : ConvertorBase<ClassNameToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var className = value as string;
            var image = BattleAnalyseImageSource.UnitImages[className];
            return image;
        }
    }
}
