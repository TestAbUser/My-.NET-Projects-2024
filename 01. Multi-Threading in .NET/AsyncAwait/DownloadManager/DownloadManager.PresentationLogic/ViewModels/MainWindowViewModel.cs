using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using DownloadManager.PresentationLogic.Commands;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.Domain;
using DownloadManager.DataAccess;
using System.Windows.Input;
using DownloadManager;
using Microsoft.Win32;
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

        private CancellationTokenSource? _cts;
        private CancellationToken _cancellationToken;

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
            _openFileCommand ??= new RelayCommand(LoadUrls, CanOpenAddWindowCommand);

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

        public CancellationToken CancellationToken
        {
            get => _cancellationToken;
            set
            {
                if (value == _cancellationToken) return;
                _cancellationToken = value;
                OnPropertyChanged(nameof(CancellationToken));
            }
        }

        public CancellationTokenSource? Cts
        {
            get => _cts;
            set
            {
                if (value == _cts) return;
                _cts = value;
                OnPropertyChanged(nameof(Cts));
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

        private CancellationToken InitializeToken(CancellationToken ct)
        {
            _cts = new CancellationTokenSource();
           // if (ct == default)
           // {
                
                ct = _cts.Token;
           // }
            return ct;
        }

        private async Task DownloadPagesAsync(CancellationToken token)
        {
            foreach (var url in Urls)
                url.Status = "Ready";
            //InitializeToken(token);
            int count = 0;
            StatusBarText = "Downloading...";
            try
            {
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
               // _cts.Dispose();
               // _cts = null;
               // token = default;
                StatusBarText = null;
                // DownloadCommand.NotifyCanExecuteChanged();
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
            // _cts?.Cancel();
            // _cts?.Dispose();
            StatusBarText = null;
        }

        private bool CanOpenAddWindowCommand(AddUrlViewModel auvm) =>
            !DownloadCommand.IsRunning;
        // _cts == null || _cts.IsCancellationRequested;

        private bool CanOpenAddWindowCommand() => _cts == null || _cts.IsCancellationRequested;

        // Download button is enabled if the urls are displayed and download process isn't in progress.
        private bool CanDownloadPages() => Urls.Count > 0 && !DownloadCommand.IsRunning;//(_cts == null || _cts.IsCancellationRequested);

        // Cancel button is enabled only if download is in progress.
        private bool CanCancelDownload() => DownloadCommand.CanBeCanceled;
            //_cts != null && !_cts.IsCancellationRequested;

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
