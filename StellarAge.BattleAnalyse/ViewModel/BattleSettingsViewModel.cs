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
        public List<UnitsView> AttackUnits => BattleSettingsItem.AttackUnits;
        public List<UnitsView> DefenceUnits => BattleSettingsItem.DefenceUnits;
        public List<UnitsView> DefenceTurrels => BattleSettingsItem.DefenceTurrels;

        public BattleSettingsViewModel()
        {
            _battleService = new BattleService();
            BattleSettingsItem = new BattleSettingsItem
            {
                AttackUnits = _battleService.GetDefaultShipList(),
                DefenceUnits = _battleService.GetDefaultShipList(),
                DefenceTurrels = _battleService.GetDefaultTurrel()
            };
            var savedItem = SaveToFile.DeSerializeObject<BattleSettingsItem>(StorFileName);
            if (savedItem != null)
            {
                InitViewUnits(savedItem.AttackUnits, AttackUnits);
                InitViewUnits(savedItem.DefenceUnits, DefenceUnits);
                InitViewUnits(savedItem.DefenceTurrels, DefenceTurrels);
            }
#if DEBUG
            _battleService.ExecuteBattle(BattleSettingsItem);
#endif
        }

        void InitViewUnits(List<UnitsView> savedUnits, List<UnitsView> currentUnits)
        {
            foreach (var unit in currentUnits)
            {
                var saveItem = savedUnits.FirstOrDefault(pp => pp.Name == unit.Name);
                if (saveItem == null) continue;
                Mapper.Map(saveItem, unit);
            }

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
                                                                   new ExtendedRelayCommand<CancelEventArgs>(ExecuteSaveCommand, CanExecuteSaveCommand,"Сохранить настройки"));

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