using System.Windows;
using DownloadManager.Commands;
using System.Collections.ObjectModel;
using DownloadManager.Models;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        CancellationTokenSource cts;

        private RelayCommand _openAddWindowCommand = null;
        private RelayCommand _openCommand = null;
        private RelayCommand _saveCommand = null;
        private RelayCommand _downloadCommand = null;
        private RelayCommand _cancelCommand = null;
        private string _statusBarText;
        private int _progressReport;
        // private bool _isEnabled;
        private bool _isChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ProgressReport //{ get; set; } = new();
        {
            get => _progressReport;
            set
            {
                if (value == _progressReport) return;
                _progressReport = value;
                OnPropertyChanged();
            }
        }

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (value == _isChanged) return;
                _isChanged = value;
                OnPropertyChanged();
            }
        }

        public string StatusBarText
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
        public ObservableCollection<string> Urls { get; } = new();


        public RelayCommand OpenAddWindowCommand => _openAddWindowCommand ??= new RelayCommand(() =>
        {
            // Pass the collection holding Urls to the other window's view model.
            AddUrlWindowViewModel viewModel = new(Urls);
            AddUrlWindow auw = new(viewModel);
            auw.ShowDialog();
        }, CanOpenAddWindowCommand);

        private bool CanOpenAddWindowCommand() => cts == null || cts.IsCancellationRequested;

        public RelayCommand OpenCommand => _openCommand ??= new RelayCommand(OpenDialog, CanOpenAddWindowCommand);


        private void OpenDialog()
        {
            // Create an open file dialog box and only show text files.
            var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };

            // Did they click on the OK button?
            if (true == openDlg.ShowDialog())
            {
                try
                {
                    // Load all text of selected file.
                    string[] dataFromFile = File.ReadAllLines(openDlg.FileName);

                    // Show Urls in DataGrid. 
                    foreach (var line in dataFromFile) { Urls.Add(line); }
                    // IsEnabled = true;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public RelayCommand SaveCommand =>
           _saveCommand ??= new RelayCommand(SaveDialog);

        private void SaveDialog()
        {
            var saveDlg = new SaveFileDialog { Filter = "Text Files |*.txt" };
            // Did they click on the OK button?
            if (true == saveDlg.ShowDialog())
            {
                try
                {
                    // Save data in the DataGrid to the named file.
                    File.WriteAllLines(saveDlg.FileName, Urls.Select(x => x));
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public RelayCommand DownloadCommand =>
         _downloadCommand ??= new RelayCommand(DownloadPages, CanDownloadPages);

        private async void DownloadPages()
        {
            string[] addresses = Urls.Select(x => x).ToArray();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            StatusBarText = "Downloading...";
            try
            {
                var progressIndicator = new Progress<int>(percent => ProgressReport = percent);
                await Downloader.Download(addresses, progressIndicator, token);
            }

            catch (OperationCanceledException ex)
            {
                MessageBox.Show("Download cancelled");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something's wrong with the address!");
            }

            cts = null;
            StatusBarText = null;
            DownloadCommand.RaiseCanExecuteChanged();
            OpenAddWindowCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            OpenCommand.RaiseCanExecuteChanged();
        }

        // Download button is enabled if the urls are displayed and download process isn't in progress.
        private bool CanDownloadPages() => Urls.Count > 0 && (cts == null || cts.IsCancellationRequested);


        public RelayCommand CancelCommand => _cancelCommand ??= new(CancelDownloading, CanCancelDownload);
        private void CancelDownloading()
        {
            cts.Cancel();
            cts.Dispose();
            StatusBarText = null;
        }

        // Cancel button is enabled only if download is in progress.
        private bool CanCancelDownload() => cts != null && !cts.IsCancellationRequested;


        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
            // The first parameter ("this") is the object instance that is raising the event.
            // The PropertyChangedEventArgs constructor takes a string that indicates the property
            // that was changed and needs to be updated. When String.Empty is used all of the bound
            // properties of the instance are updated.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
