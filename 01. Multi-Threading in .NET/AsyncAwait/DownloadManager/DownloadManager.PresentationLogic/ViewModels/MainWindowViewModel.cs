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

namespace DownloadManager.PresentationLogic.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IWindow _window;
       // private readonly IEnumerable<string> _urls;
        private readonly Persister _persister;

        CancellationTokenSource? cts;

        private RelayCommand? _addUrlCommand;
        private RelayCommand? _openFileCommand;
        private RelayCommand? _saveCommand;
        private RelayCommand? _downloadPageCommand;
        private RelayCommand? _cancelCommand;
        private string? _statusBarText;
        private double _progressReport;


        public MainWindowViewModel(//IEnumerable<string> urls,
            Persister persister,
            IWindow window)
        {
           // if (urls == null) throw new ArgumentNullException(nameof(urls));
            if (persister == null) throw new ArgumentNullException(nameof(persister));
            if (window == null) throw new ArgumentNullException(nameof(window));

            _window = window;
           // _urls = urls;
            _persister = persister;
        }

        public ICommand DownloadCommand =>
            _downloadPageCommand ??= new RelayCommand(DownloadPagesAsync, CanDownloadPages);

        public ICommand CancelCommand =>
            _cancelCommand ??= new RelayCommand(CancelDownloading, CanCancelDownload);

        public ICommand AddUrlCommand =>
            _addUrlCommand ??= new RelayCommand(AddUrl, CanOpenAddWindowCommand);

        public ICommand LoadUrlsCommand =>
            _openFileCommand ??= new RelayCommand(LoadUrls, CanOpenAddWindowCommand);
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


        // Opens AddWindow using dependency injection.
        private void AddUrl()
        {
            // Pass the collection holding Urls to the other window's view model.
            AddUrlViewModel viewModel = new(Urls);
            if (_window.CreateChild(viewModel).ShowDialogue() ?? false)
            {
                viewModel.OkClicked();
            }
           // AddUrlWindow auw = new(viewModel);
           // auw.ShowDialog();
        }

        // Opens Dialog window to load a file.
        private void LoadUrls()
        {
            var urls = _persister.LoadUrls();

           // if (dataFromFile != null)
           // {
                // Show Urls in DataGrid. 
                foreach (var line in urls)
                {
                    Urls.Add(new UrlModel { Url = line, Status = "Ready" });
                }
           // }
        }

        private void SaveUrls()
        {
            var listOfUrls = Urls.Select(x => x.Url);
            _persister.SaveUrls(listOfUrls);
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
                List<string> results = new List<string>();//await _pageRepo.DownloadAsync(addresses, token, progressIndicator);
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
