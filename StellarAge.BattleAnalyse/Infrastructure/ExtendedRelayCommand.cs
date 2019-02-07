using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace StellarAge.BattleAnalyse.Infrastructure
{
    public class ExtendedRelayCommand<T> : RelayCommand<T>
    {
        public ExtendedRelayCommand(Action<T> execute) : base(execute)
        {
        }

        public ExtendedRelayCommand(Action<T> execute, object toolTip) : this(execute, null, toolTip)
        {
        }

        public ExtendedRelayCommand(Action<T> execute, Func<T, bool> canExecute) : base(execute, canExecute)
        {
        }

        public ExtendedRelayCommand(Action<T> execute, Func<T, bool> canExecute, object toolTip) : base(execute, canExecute)
        {
            ToolTip = toolTip;
        }

        public object ToolTip { get; set; }
    }
}
