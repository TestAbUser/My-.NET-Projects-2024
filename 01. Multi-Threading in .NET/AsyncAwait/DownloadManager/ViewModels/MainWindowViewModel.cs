using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DownloadManager.Models;
using DownloadManager.Commands;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel
    {
        CancellationTokenSource cts;

        private RelayCommand _addCommand = null;

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand(() =>
                {
                    AddUrlWindow auw = new();
                    auw.ShowDialog();
                });
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
