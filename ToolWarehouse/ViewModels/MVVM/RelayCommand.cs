using System;
using System.Windows.Input;
namespace FixtureLog.ViewModels.MVVM/*для MVVM для создания команд на кнопку*/
{
    public class RelayCommand<T> : ICommand
    {
        Action<T> m_execute;
        Func<T, bool> m_canExecute;

        // execute - выполняемое действие
        // canExecute - возвращает true, если команда может выполнить действие при текущих условиях
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            m_execute = execute;
            m_canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (m_canExecute == null)
            {
                return true;
            }
            else
            {
                return m_canExecute((T)parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            m_execute((T)parameter);
        }
    }
}
