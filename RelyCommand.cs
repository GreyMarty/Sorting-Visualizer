using System;
using System.Windows.Input;


namespace SortingVisualizer
{
    internal class RelyCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action _action;


        public RelyCommand(Action action) 
        {
            _action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _action.Invoke();
        }
    }
}
