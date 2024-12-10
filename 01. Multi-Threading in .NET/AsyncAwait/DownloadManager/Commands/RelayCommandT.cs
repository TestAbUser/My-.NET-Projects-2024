namespace DownloadManager.Commands
{
    // Is used when CanExecute() and Execute() methods take a parameter.
    public class RelayCommand<T>: RelayCommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }
        public RelayCommand(
        Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute
            ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        //  Determines whether the command can execute in its current state.
        public override  bool CanExecute(object? parameter) => _canExecute == null || _canExecute((T)parameter);
       
        // Is called when the command is invoked.
        public override void Execute(object? parameter)
        { 
            _execute((T)parameter); 
        }
    }
}
