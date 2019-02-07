using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using StellarAge.BattleAnalyse.Log;
using StellarAge.BattleAnalyse.Model.Battle;
using StellarAge.BattleAnalyse.Model.Common;
using StellarAge.BattleAnalyse.Model.Ships;
using StellarAge.BattleAnalyse.Model.Turrels;
using StellarAge.BattleAnalyse.ViewModel;

namespace StellarAge.BattleAnalyse.Services
{
    public class BattleService
    {
        public List<UnitsView> GetDefaultShipList()
        {
            var ret = GetDefaultUnitsView(typeof(Ship));
            return ret;
        }
        public List<UnitsView> GetDefaultTurrel()
        {
            var ret = GetDefaultUnitsView(typeof(Turrel));
            return ret;
        }
        public List<UnitsView> GetDefaultUnitsView(Type unitType)
        {
            var ret = new List<UnitsView>();
            var units = GetDefaultUnits(unitType);
            foreach (var unit in units)
            {
                var viewItem = new UnitsView
                {
                    Name = unit.Name,
                    Weight = unit.Weight,
                    ClassName = unit.GetType().Name
                };
                ret.Add(viewItem);
            }
            return ret;
        }

        List<Unit> GetDefaultUnits(Type unitType)
        {
            var itemTypes = Assembly
                .GetAssembly(unitType)
                .GetTypes()
                .Where(t => t.IsSubclassOf(unitType))
                .ToList();
            var ret = itemTypes.Select(p => (Unit)Activator.CreateInstance(p)).ToList();
            return ret.OrderBy(p => p.Weight).ToList();
        }

        public LogBattle ExecuteBattle(BattleSettingsItem battleSettingsItem)
        {
            var battle = new Battle();
            battle.AttackFleet = GetUnitsFromViewGroups(battleSettingsItem.AttackUnits);
            battle.DefenceFleet = GetUnitsFromViewGroups(battleSettingsItem.DefenceUnits);
            battle.DefenceTurrels = GetUnitsFromViewGroups(battleSettingsItem.DefenceTurrels);
           
            var ret = battle.SimBattle();
            return ret;
        }

        List<Unit> GetUnitsFromViewGroups(List<UnitsView> unitsView)
        {
            var ret = new List<Unit>();
            foreach (var unitView in unitsView)
            {
                var unitsGroup = GetUnitsFromView(unitView);
                ret.AddRange(unitsGroup);
            }
            return ret;
        }
        List<Unit> GetUnitsFromView(UnitsView unitView)
        {
            var ret=new List<Unit>();
            var unitType = Assembly
                .GetAssembly(GetType())
                .GetTypes().FirstOrDefault(t => t.Name == unitView.ClassName);
            if (unitType == null) return ret;
            for (var i = 0; i < unitView.Count; i++)
            {
                var newUnit = (Unit)Activator.CreateInstance(unitType);
                Mapper.Map(unitView, newUnit);
                ret.Add(newUnit);
            }
            return ret;
        }
    }
}
