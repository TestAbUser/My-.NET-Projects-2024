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
      // private AddUrlWindowViewModel _auwViewModel;
        public IList<string> Urls { get; set; }
        CancellationTokenSource cts;

        private RelayCommand _openAddWindowCommand = null;

        public MainWindowViewModel()
        {
        }
        public MainWindowViewModel(AddUrlWindowViewModel auwViewModel)
        {
            Urls = auwViewModel.Urls;
        }

        public RelayCommand OpenAddWindowCommand
        {
            get
            {
                return _openAddWindowCommand ??= new RelayCommand(() =>
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
