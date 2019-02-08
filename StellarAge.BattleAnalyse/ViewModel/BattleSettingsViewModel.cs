using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using AutoMapper;
using GalaSoft.MvvmLight.CommandWpf;
using StellarAge.BattleAnalyse.Infrastructure;
using StellarAge.BattleAnalyse.Services;

namespace StellarAge.BattleAnalyse.ViewModel
{
    class BattleSettingsViewModel
    {
        private readonly BattleService _battleService;
        private ExtendedRelayCommand<CancelEventArgs> _closingCommand;
        private ExtendedRelayCommand<CancelEventArgs> _startSimulationCommand;
        private string _storFileName;
        private ExtendedRelayCommand<CancelEventArgs> _saveCommand;

        public BattleSettingsItem BattleSettingsItem { get; set; }
        public List<HandView> AttackHands => BattleSettingsItem.AttackHands;
        public List<HandView> DefenceHands => BattleSettingsItem.DefenceHands;
        public List<UnitsView> DefenceTurrels => BattleSettingsItem.DefenceTurrels;

        public BattleSettingsViewModel()
        {
            _battleService = new BattleService();
            BattleSettingsItem = RestoreFromFile() ?? InitDefaultValues();
#if DEBUG
            _battleService.ExecuteBattle(BattleSettingsItem);
#endif
        }

        private BattleSettingsItem InitDefaultValues()
        {
            var ret = new BattleSettingsItem
            {
                AttackHands = new List<HandView> { new HandView { UnitsView = _battleService.GetDefaultShipList() } },
                DefenceHands = new List<HandView> { new HandView { UnitsView = _battleService.GetDefaultShipList() } },
                DefenceTurrels = _battleService.GetDefaultTurrel()
            };
            return ret;
        }

        private BattleSettingsItem RestoreFromFile()
        {
            var savedItem = SaveToFile.DeSerializeObject<BattleSettingsItem>(StorFileName);
            return savedItem;
        }

        private string StorFileName => _storFileName ?? (_storFileName = Path.Combine(Environment.GetFolderPath(
                                               Environment.SpecialFolder.ApplicationData),
                                           "StellarAge.BattleAnalyse.BattleSettingsItem.data"));


        public ExtendedRelayCommand<CancelEventArgs> ClosingCommand => _closingCommand ?? (_closingCommand =
                                                                   new ExtendedRelayCommand<CancelEventArgs>(ExecuteClosingCommand, CanExecuteClosingCommand));

        private bool CanExecuteClosingCommand(CancelEventArgs arg)
        {
            return true;
        }

        private void ExecuteClosingCommand(CancelEventArgs obj)
        {
            SaveToFile.SerializeObject(BattleSettingsItem, StorFileName);
        }

        public ExtendedRelayCommand<CancelEventArgs> SaveCommand => _saveCommand ?? (_saveCommand =
                                                                   new ExtendedRelayCommand<CancelEventArgs>(ExecuteSaveCommand, CanExecuteSaveCommand, "Сохранить настройки"));

        private bool CanExecuteSaveCommand(CancelEventArgs arg)
        {
            return true;
        }

        private void ExecuteSaveCommand(CancelEventArgs obj)
        {
            SaveToFile.SerializeObject(BattleSettingsItem, StorFileName);
        }

        public ExtendedRelayCommand<CancelEventArgs> StartSimulationCommand => _startSimulationCommand ?? (_startSimulationCommand =
                                                                   new ExtendedRelayCommand<CancelEventArgs>(ExecuteStartSimulationCommand, CanExecuteStartSimulationCommand, "Запустить симуляцию"));

        private bool CanExecuteStartSimulationCommand(CancelEventArgs arg)
        {
            return true; // BattleSettingsItem.ReadyForSimulation();
        }

        private void ExecuteStartSimulationCommand(CancelEventArgs obj)
        {
            _battleService.ExecuteBattle(BattleSettingsItem);
        }
    }
}