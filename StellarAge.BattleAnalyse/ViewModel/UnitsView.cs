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
        [DataMember] private long _unitArmor;
        [DataMember] private long _unitAttackPower;
        [DataMember] private long _count;
        private long _totalArmor;
        private long _totalAttackPower;

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

        public long UnitArmor
        {
            get => _unitArmor;
            set
            {
                if (_unitArmor == value) return;
                _unitArmor = value;
                NotifyPropertyChanged();
                if (Count <= 0) return;
                _totalArmor = _unitArmor * Count;
                NotifyPropertyChanged(nameof(TotalArmor));
            }
        }

        public long UnitAttackPower
        {
            get => _unitAttackPower;
            set
            {

                if (_unitAttackPower == value) return;
                _unitAttackPower = value;
                NotifyPropertyChanged();
                if (Count <= 0) return;
                _totalAttackPower = _unitAttackPower * Count;
                NotifyPropertyChanged(nameof(TotalAttackPower));
            }
        }

        public long TotalArmor
        {
            get => _totalArmor;
            set
            {
                if (_totalArmor == value) return;
                _totalArmor = value;
                NotifyPropertyChanged();
                if (Count <= 0) return;
                _unitArmor = _totalArmor / Count;
                NotifyPropertyChanged(nameof(UnitArmor));
            }
        }

        public long TotalAttackPower
        {
            get => _totalAttackPower;
            set
            {
                if (_totalAttackPower == value) return;
                _totalAttackPower = value;
                NotifyPropertyChanged();
                if (Count <= 0) return;
                _unitAttackPower = _totalAttackPower / Count;
                NotifyPropertyChanged(nameof(UnitAttackPower));
            }
        }

        public long Count
        {
            get => _count;
            set
            {
                _count = value;
                NotifyPropertyChanged();
                if (Count <= 0) return;
                _totalAttackPower = _unitAttackPower * Count;
                NotifyPropertyChanged(nameof(TotalAttackPower));
                _totalArmor = _unitArmor * Count;
                NotifyPropertyChanged(nameof(TotalArmor));
            }
        }
    }
}