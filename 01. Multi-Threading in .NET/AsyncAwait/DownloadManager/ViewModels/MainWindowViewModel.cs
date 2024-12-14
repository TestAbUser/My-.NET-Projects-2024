using System.Windows;
using DownloadManager.Commands;
using System.Collections.ObjectModel;
using DownloadManager.Models;
using Microsoft.Win32;
using System.IO;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel
    {
        CancellationTokenSource cts;

        private RelayCommand _openAddWindowCommand = null;
        private RelayCommand _openCommand = null;

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
            _openCommand ??= new RelayCommand(OpenDialog, null);
       
        private void OpenDialog()
        {
            // Create an open file dialog box and only show text files.
            var openDlg = new OpenFileDialog { Filter = "Text Files |*.txt" };
            // Did they click on the OK button?
            if (true == openDlg.ShowDialog())
            {
                // Load all text of selected file.
                string[] dataFromFile = File.ReadAllLines(openDlg.FileName);
                
                // Show Urls in DataGrid. 
                foreach(var line in dataFromFile) { Urls.Add(line); }
            }
        }

        private async void DownloadPages(object sender, RoutedEventArgs e)
        {
            //string[] addresses = addWindowTextBox.Text.Split(',');
            //cts = new CancellationTokenSource();
            //CancellationToken token = cts.Token;
            //var t = Task.Run(async () => await Downloader.Download(addresses, token));
            //statBarText.Text = "Downloading...";
            //downloadBtn.IsEnabled = false;
            //cancelBtn.IsEnabled = true;
            //await t;
            //await Downloader.Download(addresses, token);
            //cancelBtn.IsEnabled = false;
            //downloadBtn.IsEnabled = true;
            //statBarText.Text = "Ready";
        }

        private void CancelDownloading(object sender, RoutedEventArgs e)
        {
            //cts.Cancel();
            //cts.Dispose();
            //cancelBtn.IsEnabled = false;
            //downloadBtn.IsEnabled = true;
            //statBarText.Text = "Ready";
        }
    }
}
