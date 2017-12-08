using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TripCalculator.Models
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _commandFunction;
        private Func<object, bool> _canExecuteCommandFunction;

        public Command(Action<object> commandFunction, Func<object, bool> canExecuteFunction)
        {
            _commandFunction = commandFunction;
            _canExecuteCommandFunction = canExecuteFunction;
        }

        /// <summary>
        /// If no function exists, defaults to false (cannot execute)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteCommandFunction == null ? false : _canExecuteCommandFunction.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                _commandFunction?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}

