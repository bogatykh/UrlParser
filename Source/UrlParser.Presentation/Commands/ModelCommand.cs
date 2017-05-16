using System;

namespace UrlParser.Presentation.Commands
{
    internal class ModelCommand<TParameter> : CommandBase
        where TParameter : class
    {
        private readonly Action<TParameter> _execute;
        private readonly Predicate<TParameter> _canExecute;

        public ModelCommand(Action<TParameter> execute)
            : this(execute, null)
        {
        }

        public ModelCommand(Action<TParameter> execute, Predicate<TParameter> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute((TParameter)parameter);
            }
            return true;
        }

        public override void Execute(object parameter)
        {
            _execute((TParameter)parameter);
        }
    }
}
