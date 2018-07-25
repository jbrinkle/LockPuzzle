using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuzzleHunt2018.ViewModels
{
    internal class DelegateCommand : ICommand
    {
        private Action executeMethod;
        private Func<bool> canExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action commandAction) : this(commandAction, null)
        { }

        public DelegateCommand(Action commandAction, Func<bool> commandAllowedDeterminer)
        {
            executeMethod = commandAction ?? throw new ArgumentNullException(nameof(commandAction));
            canExecuteMethod = commandAllowedDeterminer ?? (() => true);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter = null)
        {
            return canExecuteMethod();
        }

        public void Execute(object parameter = null)
        {
            if (CanExecute())
            {
                executeMethod();
            }
        }
    }

    internal class DelegateCommand<T> : ICommand
    {
        private Action<T> executeMethod;
        private Func<T, bool> canExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> commandAction) : this(commandAction, null)
        { }

        public DelegateCommand(Action<T> commandAction, Func<T, bool> commandAllowedDeterminer)
        {
            executeMethod = commandAction ?? throw new ArgumentNullException(nameof(commandAction));
            canExecuteMethod = commandAllowedDeterminer ?? (T => true);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteMethod((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute((T)parameter))
            {
                executeMethod((T)parameter);
            }
        }
    }
}
