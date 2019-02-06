using System.Windows;
using StellarAge.BattleAnalyse.ViewModel;

namespace StellarAge.BattleAnalyse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Bootstrap.Init();
            InitializeComponent();
            DataContext = new BattleSettingsViewModel();
        }
    }
}
