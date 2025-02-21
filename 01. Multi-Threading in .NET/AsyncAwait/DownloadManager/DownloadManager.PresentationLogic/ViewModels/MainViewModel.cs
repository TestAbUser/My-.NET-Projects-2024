using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using DownloadManager.PresentationLogic.Commands;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.Domain;
using System.Windows.Input;
using DownloadManager;
using Microsoft.Win32;

namespace DownloadManager.PresentationLogic.ViewModels
{
    public class MainViewModel : IViewModel, INotifyPropertyChanged
    {
        private IWindow _window;
        private readonly IUrlRepository _urlRepo;
        private readonly IPageRepository _pageRepo;

        CancellationTokenSource? cts;

        private RelayCommand? _openAddWindowCommand;
        private RelayCommand? _openFileCommand;
        private RelayCommand? _saveCommand;
        private RelayCommand? _downloadPageCommand;
        private RelayCommand? _cancelCommand;
        private string? _statusBarText;
        private double _progressReport;


        public MainViewModel(//IUrlRepository urlRepo,
           // IPageRepository pageRepo,
            IWindow window)
        {
           // if (urlRepo == null) throw new ArgumentNullException(nameof(urlRepo));
           // if (pageRepo == null) throw new ArgumentNullException(nameof(pageRepo));
            if (window == null) throw new ArgumentNullException(nameof(window));

            _window = window;
            //_urlRepo = urlRepo;
            //_pageRepo = pageRepo;
        }

        public ICommand DownloadCommand =>
            _downloadPageCommand ??= new RelayCommand(DownloadPagesAsync, CanDownloadPages);

        public ICommand CancelCommand =>
            _cancelCommand ??= new RelayCommand(CancelDownloading, CanCancelDownload);

        public ICommand OpenAddWindowCommand =>
            _openAddWindowCommand ??= new RelayCommand(AddUrl, CanOpenAddWindowCommand);

        public ICommand OpenFileCommand =>
            _openFileCommand ??= new RelayCommand(OpenFileWithUrls, CanOpenAddWindowCommand);
        public ICommand SaveCommand =>
            _saveCommand ??= new RelayCommand(SaveUrls);


        public event PropertyChangedEventHandler? PropertyChanged;

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

        public void Initialize(Action? action, object? model)
        {
            
        }

        // Opens AddWindow using dependency injection.
        private void AddUrl()
        {
            // Pass the collection holding Urls to the other window's view model.
            AddUrlViewModel viewModel = new(Urls);
            if (_window.CreateChild(viewModel).ShowDialogue() ?? false)
            {

            }
           // AddUrlWindow auw = new(viewModel);
           // auw.ShowDialog();
        }

        // Opens Dialog window to load a file.
        private void OpenFileWithUrls()
        {
            var dataFromFile = _urlRepo.LoadFileContent();

            if (dataFromFile != null)
            {
                // Show Urls in DataGrid. 
                foreach (var line in dataFromFile)
                {
                    Urls.Add(new UrlModel { Url = line, Status = "Ready" });
                }
            }
        }


        private void SaveUrls()
        {
            var listOfUrls = Urls.Select(x => x.Url);
            _urlRepo.SaveDialog(listOfUrls);
        }


        private async void DownloadPagesAsync()
        {
            //StringDownloader stringDownloader = new StringDownloader();
            string[] addresses = Urls.Select(x => x.Url).ToArray();
            foreach (var url in Urls)
                url.Status = "Ready";
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var count = 0;
            StatusBarText = "Downloading...";
            try
            {
                var progressIndicator = new Progress<ValueTuple<double, string>>(progress =>
                {
                    ProgressReport = progress.Item1; // updates progress bar
                    Urls.ElementAt(count).Status = progress.Item2; // updates status values
                    count++;
                });
                List<string> results = await _pageRepo.DownloadAsync(addresses, token, progressIndicator);
            }
            finally
            {
                ProgressReport = 0;
                cts.Dispose();
                cts = null;
                StatusBarText = null;
                ((RelayCommand)DownloadCommand).RaiseCanExecuteChanged();
            }
        }

        private void CancelDownloading()
        {
            cts?.Cancel();
            cts?.Dispose();
            StatusBarText = null;
        }

        private bool CanOpenAddWindowCommand() => cts == null || cts.IsCancellationRequested;

        // Download button is enabled if the urls are displayed and download process isn't in progress.
        private bool CanDownloadPages() => Urls.Count > 0 && (cts == null || cts.IsCancellationRequested);

        // Cancel button is enabled only if download is in progress.
        private bool CanCancelDownload() => cts != null && !cts.IsCancellationRequested;

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
