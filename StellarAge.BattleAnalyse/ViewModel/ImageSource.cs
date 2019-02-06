using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StellarAge.BattleAnalyse.ViewModel
{
    static class BattleAnalyseImageSource
    {
        private static UnitImages _unitImages;
        public static UnitImages UnitImages => _unitImages ?? (_unitImages = new UnitImages());
    }

    class UnitImages
    {
        private ImageSource _BB;
        private ImageSource _DR;
        private ImageSource _ES;
        private ImageSource _KR;
        private ImageSource _LI;
        private ImageSource _LK;
        private ImageSource _LT;
        private ImageSource _TI;
        private ImageSource _TN;
        private ImageSource _TT;
        private ImageSource _GU;
        private ImageSource _IU;
        private ImageSource _LU;
        private ImageSource _PU;
        private ImageSource _RU;

        public ImageSource BB => _BB ?? (_BB = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/BB.png", UriKind.Relative)));
        public ImageSource DR => _DR ?? (_DR = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/DR.png", UriKind.Relative)));
        public ImageSource ES => _ES ?? (_ES = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/ES.png", UriKind.Relative)));
        public ImageSource KR => _KR ?? (_KR = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/KR.png", UriKind.Relative)));
        public ImageSource LI => _LI ?? (_LI = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/LI.png", UriKind.Relative)));
        public ImageSource LK => _LK ?? (_LK = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/LK.png", UriKind.Relative)));
        public ImageSource LT => _LT ?? (_LT = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/LT.png", UriKind.Relative)));
        public ImageSource TI => _TI ?? (_TI = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/TI.png", UriKind.Relative)));
        public ImageSource TN => _TN ?? (_TN = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/TN.png", UriKind.Relative)));
        public ImageSource TT => _TT ?? (_TT = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/TT.png", UriKind.Relative)));
        public ImageSource GU => _GU ?? (_GU = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/GU.png", UriKind.Relative)));
        public ImageSource IU => _IU ?? (_IU = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/IU.png", UriKind.Relative)));
        public ImageSource LU => _LU ?? (_LU = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/LU.png", UriKind.Relative)));
        public ImageSource PU => _PU ?? (_PU = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/PU.png", UriKind.Relative)));
        public ImageSource RU => _RU ?? (_RU = new BitmapImage(new Uri("/StellarAge.BattleAnalyse;component/Images/RU.png", UriKind.Relative)));

        public ImageSource this[string propertyName] => (BitmapImage)GetType().GetProperty(propertyName)?.GetValue(this, null);
    }
}
