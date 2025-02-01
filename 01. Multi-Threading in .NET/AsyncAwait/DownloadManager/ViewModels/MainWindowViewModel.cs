using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using DownloadManager.Commands;
using DownloadManager.Models;
using DownloadManager.Helpers;
using Microsoft.Win32;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged//, IFileSystem
    {
        private readonly IFileSystem _fileSystem;

        CancellationTokenSource? cts;

        private RelayCommand? _openAddWindowCommand;
        private RelayCommand? _openCommand;
        private RelayCommand? _saveCommand;
        private RelayCommand? _downloadCommand;
        private RelayCommand? _cancelCommand;
        private string? _statusBarText;
        private double _progressReport;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

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
        public RelayCommand OpenAddWindowCommand => _openAddWindowCommand ??= new RelayCommand(() =>
        {
            // Pass the collection holding Urls to the other window's view model.
            AddUrlWindowViewModel viewModel = new(Urls);
            AddUrlWindow auw = new(viewModel);
            auw.ShowDialog();
        }, CanOpenAddWindowCommand);

        private bool CanOpenAddWindowCommand() => cts == null || cts.IsCancellationRequested;

        // Opens Dialog window to load a file.
        public RelayCommand OpenCommand => _openCommand ??= new RelayCommand(() =>
        {
            var dataFromFile = _fileSystem.LoadFileContent();

            if (dataFromFile != null)
            {
                // Show Urls in DataGrid. 
                foreach (var line in dataFromFile)
                {
                    Urls.Add(new UrlModel { Url = line, Status = "Ready" });
                }
            }
        }, CanOpenAddWindowCommand);
       
        public RelayCommand SaveCommand => _saveCommand ??= new RelayCommand(() =>
        {
            var listOfUrls = Urls.Select(x => x.Url);
            _fileSystem.SaveDialog(listOfUrls);
        });

       
        public RelayCommand DownloadCommand =>
         _downloadCommand ??= new RelayCommand(DownloadPagesAsync, CanDownloadPages);

        private async void DownloadPagesAsync()
        {
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
                List<string> results = await Downloader.DownloadAsync(addresses, token, progressIndicator);
            }
            finally
            {
                ProgressReport = 0;
                cts.Dispose();
                cts = null;
                StatusBarText = null;
                DownloadCommand.RaiseCanExecuteChanged();
            }
        }

        // Download button is enabled if the urls are displayed and download process isn't in progress.
        private bool CanDownloadPages() => Urls.Count > 0 && (cts == null || cts.IsCancellationRequested);

        public RelayCommand CancelCommand => _cancelCommand ??= new(CancelDownloading, CanCancelDownload);
        private void CancelDownloading()
        {
            cts?.Cancel();
            cts?.Dispose();
            StatusBarText = null;
        }

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
