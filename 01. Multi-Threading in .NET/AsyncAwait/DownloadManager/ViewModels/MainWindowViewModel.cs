using System.Windows;
using DownloadManager.Commands;
using System.Collections.ObjectModel;
using DownloadManager.Models;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        CancellationTokenSource cts;

        private RelayCommand _openAddWindowCommand = null;
        private RelayCommand _openCommand = null;
        private RelayCommand _saveCommand = null;
        private RelayCommand _downloadCommand = null;
        private RelayCommand _cancelCommand = null;
        private string _statusBarText;
        private bool _isChanged;
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

   

        public RelayCommand OpenAddWindowCommand
        {
            get
            {
                return _openAddWindowCommand ??= new RelayCommand(() =>
                {
                    // Pass the collection holding Urls to the other window's view model.
                    AddUrlWindowViewModel viewModel = new(Urls);
                    AddUrlWindow auw = new(viewModel);
                    auw.ShowDialog();
                });
            }
        }

        public RelayCommand OpenCommand =>
            _openCommand ??= new RelayCommand(OpenDialog);

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
         _downloadCommand ??= new RelayCommand(DownloadPages);

        private async void DownloadPages()
        {
            string[] addresses = Urls.Select(x=>x).ToArray();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var t = Task.Run(async () => await Downloader.Download(addresses, token));
        StatusBarText = "Downloading...";
            //downloadBtn.IsEnabled = false;
            //cancelBtn.IsEnabled = true;
           // await t;
            await Downloader.Download(addresses, token);
            //cancelBtn.IsEnabled = false;
            //downloadBtn.IsEnabled = true;
            //statBarText.Text = "Ready";
        }

        public RelayCommand CancelCommand =>
        _cancelCommand ??= new RelayCommand(CancelDownloading);
        private void CancelDownloading()
        {
            cts.Cancel();
            cts.Dispose();
            //cancelBtn.IsEnabled = false;
            //downloadBtn.IsEnabled = true;
            StatusBarText = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
