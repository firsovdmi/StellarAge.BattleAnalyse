using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StellarAge.BattleAnalyse.ViewModel
{
    [Serializable]
    public class NotifyPropertyChangedImplement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}