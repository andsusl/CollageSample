using System;
using System.Windows.Input;

namespace CollageSample.ViewModels.Utils
{
    // I don't need whole Prism frmework.
    public class DelegateCommand<T> : ICommand
    {
        readonly Action<T> m_method = null;
        readonly Predicate<object> m_canExecutePredicate = null;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<T> executeMethod, Predicate<object> canExecutePredicate = null)
        {
            m_method = executeMethod;
            m_canExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter)
        {
            return (parameter is T) && (null == m_canExecutePredicate || m_canExecutePredicate(parameter));
        }

        public void Execute(object parameter)
        {
            m_method((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (null != handler)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
