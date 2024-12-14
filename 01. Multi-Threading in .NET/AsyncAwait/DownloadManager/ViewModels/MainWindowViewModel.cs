using System.Windows;
using DownloadManager.Commands;
using System.Collections.ObjectModel;
using DownloadManager.Models;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel
    {
        CancellationTokenSource cts;

        private RelayCommand _openAddWindowCommand = null;

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

        private async void DownloadPages(object sender, RoutedEventArgs e)
        {
            string[] addresses = addWindowTextBox.Text.Split(',');
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var t = Task.Run(async () => await Downloader.Download(addresses, token));
            statBarText.Text = "Downloading...";
            downloadBtn.IsEnabled = false;
            cancelBtn.IsEnabled = true;
            await t;
            await Downloader.Download(addresses, token);
            cancelBtn.IsEnabled = false;
            downloadBtn.IsEnabled = true;
            statBarText.Text = "Ready";
        }

        private void CancelDownloading(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            cts.Dispose();
            cancelBtn.IsEnabled = false;
            downloadBtn.IsEnabled = true;
            statBarText.Text = "Ready";
        }
    }
}
