using System.Windows.Input;

namespace DownloadManager.Commands
{
    // Is used when there aren't any parameters needed for the CanExecute() and Execute() methods.
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        // Occurs when changes occur that affect whether or not the command should execute.
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Is needed by the RelayCommand<T>.
        public RelayCommand() { }
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute
            ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Is marked virtual to allow its modification in a derived class.
        public virtual bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        // Is marked virtual to allow its modification in a derived class.
        public virtual void Execute(object? parameter) { _execute(); }
    }
}
