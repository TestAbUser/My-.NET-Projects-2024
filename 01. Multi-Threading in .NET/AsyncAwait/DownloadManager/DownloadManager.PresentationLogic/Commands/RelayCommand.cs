using System.Windows.Input;

namespace DownloadManager.PresentationLogic.Commands
{
    // Is used when there aren't any parameters needed for the CanExecute() and Execute() methods.
    public class RelayCommand : ICommand
    {
        private readonly Action? _execute;
        private readonly Func<bool>? _canExecute;

        // Occurs when changes occur that affect whether or not the command should execute.
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Forces the Command to raise the CanExecuteChanged event.
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        // Is needed by the RelayCommand<T>.
        public RelayCommand() { }

        // Allows to define a command.
        public RelayCommand(Action execute) : this(execute, null) { }

        // Allows to define a command and conditions when it's available.
        public RelayCommand(Action execute, Func<bool>? canExecute)
        {
            _execute = execute
            ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Is marked virtual to allow its modification in a derived class.
        public virtual bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        // Is marked virtual to allow its modification in a derived class.
        public virtual void Execute(object? parameter) { _execute?.Invoke(); }
    }
}
