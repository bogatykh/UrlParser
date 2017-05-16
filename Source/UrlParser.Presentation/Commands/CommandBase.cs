using System;
using System.Windows.Input;

namespace UrlParser.Presentation.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected CommandBase()
        {
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
