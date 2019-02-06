using System;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace StellarAge.BattleAnalyse.ViewModel
{
    [Serializable]
    public class UnitsView : NotifyPropertyChangedImplement
    {
        [DataMember] private string _name;
        [DataMember] private long _weight;
        [DataMember] private long _armor;
        [DataMember] private long _attackPower;
        [DataMember] private long _count;

        [DataMember]
        public string ClassName { get; set; }

        public ImageSource Image => BattleAnalyseImageSource.UnitImages[ClassName];
        
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public long Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                NotifyPropertyChanged();
            }
        }

        public long Armor
        {
            get => _armor;
            set
            {
                _armor = value;
                NotifyPropertyChanged();
            }
        }

        public long AttackPower
        {
            get => _attackPower;
            set
            {
                _attackPower = value;
                NotifyPropertyChanged();
            }
        }

        public long Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyPropertyChanged();
            }
        }
    }
}