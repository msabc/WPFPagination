using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2
{
    class RelayCommand : ICommand
    {
        private Action<object> myAct;

        public RelayCommand(Action<object> act)
        {
            myAct = act;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            myAct?.Invoke(parameter);
        }
    }
}
