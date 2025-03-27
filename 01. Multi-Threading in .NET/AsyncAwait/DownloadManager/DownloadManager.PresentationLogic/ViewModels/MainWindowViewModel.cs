using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using DownloadManager.PresentationLogic.Commands;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.Domain;
using DownloadManager.DataAccess;
using System.Windows.Input;
using CToolkit = CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;

namespace DownloadManager.PresentationLogic.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IWindow _window;
        private readonly IPageRepository _pageRepository;
        private readonly IUrlPersister _urlPersister;
        private List<string> _pages;

        ObservableCollection<UrlModel> urls;

        private RelayCommand? _addUrlCommand;
        private RelayCommand? _openFileCommand;
        private RelayCommand? _saveCommand;
        // private RelayCommand? _downloadPageCommand;
        private CToolkit.AsyncRelayCommand/*<CancellationToken>*/? _downloadPageCommand;
        private RelayCommand? _cancelCommand;
        private string? _statusBarText;
        private double _progressReport;
        private AddUrlViewModel _addUrlViewModel;

        public MainWindowViewModel(IPageRepository repo,
            IUrlPersister urlPersister,
            IWindow window)
        {
            if (repo == null) throw new ArgumentNullException(nameof(repo));
            if (urlPersister == null) throw new ArgumentNullException(nameof(urlPersister));
            if (window == null) throw new ArgumentNullException(nameof(window));

            _pageRepository = repo;
            _window = window;
            _urlPersister = urlPersister;
            _addUrlViewModel = new AddUrlViewModel(Urls);
           // _cts = new CancellationTokenSource();
            _pages = new List<string>();
            // urls= new ObservableCollection<UrlModel>();
            Urls.CollectionChanged += OnUrlsCollectionChanged;
        }

        //public ICommand DownloadCommand =>
        //    _downloadPageCommand ??= new RelayCommand(DownloadPagesAsync, CanDownloadPages);

        public CToolkit.IAsyncRelayCommand DownloadCommand =>
            _downloadPageCommand ??=
            new CToolkit.AsyncRelayCommand(
                DownloadPagesAsync, CanDownloadPages);

        public ICommand CancelCommand =>
            _cancelCommand ??= new RelayCommand(CancelDownloading, CanCancelDownload);

        public ICommand AddUrlCommand =>
            _addUrlCommand ??= new RelayCommand<AddUrlViewModel>(AddUrl, CanOpenAddWindowCommand);

        public ICommand LoadUrlsCommand =>
            _openFileCommand ??= new RelayCommand(LoadUrls, CanLoadUrlsFromFile);

        public ICommand SaveCommand =>
            _saveCommand ??= new RelayCommand(SaveUrls);



        public List<string> Pages
        {
            get => _pages;
            set
            {
                if (value == _pages) return;
                _pages = value;
            }
        }

        public AddUrlViewModel AddUrlVM
        {
            get => _addUrlViewModel;
            set
            {
                if (value == _addUrlViewModel) return;
                _addUrlViewModel = value;
                OnPropertyChanged(nameof(AddUrlVM));
            }
        }

        public double ProgressReport
        {
            get => _progressReport;
            set
            {
                if (value == _progressReport) return;
                _progressReport = value;
                OnPropertyChanged(nameof(ProgressReport));
            }
        }

        public string? StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                if (value != _statusBarText)
                {
                    _statusBarText = value;
                    OnPropertyChanged(nameof(StatusBarText));
                }
            }
        }

        // ObservableCollection type notifies about changes in the collection.
        public ObservableCollection<UrlModel> Urls { get; } = [];

        private void OnUrlsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
           _downloadPageCommand?.NotifyCanExecuteChanged();
        }

        // Adds url via a modal window.
        private void AddUrl(AddUrlViewModel viewModel)
        {
            if (_window.CreateChild(viewModel).ShowDialogue() ?? false)
            {
                viewModel.ClickOk();
            }
        }

        private void LoadUrls()
        {
            var urls = _urlPersister.LoadUrls();

            if (urls != null)
            {
                //Show Urls in DataGrid.
                foreach (var line in urls)
                {
                    Urls.Add(new UrlModel { Url = line, Status = "Ready" });
                }
            }
        }

        private void SaveUrls() => _urlPersister.SaveUrlsToFile(GetArrayOfUrls());

        private string[] GetArrayOfUrls() => Urls.Select(x => x.Url).ToArray();

        private async Task DownloadPagesAsync(CancellationToken token)
        {
            foreach (var url in Urls)
                url.Status = "Ready";
            int count = 0;
            StatusBarText = "Downloading...";
            try
            {
                //await Task.Delay(500);
                var progressIndicator = DisplayProgressBarAndUrlStatus();
                Pages = await _pageRepository.DownloadPagesAsync(
                    GetArrayOfUrls(), token, progressIndicator);
                //token.ThrowIfCancellationRequested();
            }
            //catch (OperationCanceledException ex)
            //{ }
            finally
            {
                ProgressReport = 0;
                StatusBarText = null;
                ((RelayCommand)CancelCommand).RaiseCanExecuteChanged();
               // ((CToolkit.AsyncRelayCommand)DownloadCommand).NotifyCanExecuteChanged();//.RaiseCanExecuteChanged();
            }

            Progress<ValueTuple<double, string>> DisplayProgressBarAndUrlStatus()
            {
                var progressIndicator = new Progress<ValueTuple<double, string>>(progress =>
                {
                    ProgressReport = progress.Item1; // updates progress bar
                    Urls.ElementAt(count).Status = progress.Item2; // updates status values
                    count++;
                });
                return progressIndicator;
            }
        }


        private void CancelDownloading()
        {
            _downloadPageCommand?.Cancel();
            StatusBarText = null;
        }

        private bool CanOpenAddWindowCommand(AddUrlViewModel auvm) =>!DownloadCommand.IsRunning;

          private bool CanLoadUrlsFromFile() => !DownloadCommand.IsRunning;

        // Download button is enabled if the urls are displayed and download process isn't in progress.
        private bool CanDownloadPages() => Urls.Count > 0 && !DownloadCommand.IsRunning;//(_cts == null || _cts.IsCancellationRequested);

        private bool CanCancelDownload() => DownloadCommand.CanBeCanceled;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            // The first parameter ("this") is the object instance that is raising the event.
            // The PropertyChangedEventArgs constructor takes a string that indicates the property
            // that was changed and needs to be updated. When String.Empty is used all of the bound
            // properties of the instance are updated.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
