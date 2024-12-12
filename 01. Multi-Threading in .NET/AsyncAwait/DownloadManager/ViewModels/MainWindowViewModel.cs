using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DownloadManager.Models;
using DownloadManager.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        // private AddUrlWindowViewModel _auwViewModel;
        //public ObservableCollection<string> Urls { get; set; } = new ObservableCollection<string>();
        CancellationTokenSource cts;
        private readonly UrlModel _urlModel = new UrlModel();

        private RelayCommand _openAddWindowCommand = null;

        public ObservableCollection<string> Urls { get;private set; }

        public MainWindowViewModel()
        {
            Urls= _urlModel.Urls;
        }


        public RelayCommand OpenAddWindowCommand
        {
            get
            {
                return _openAddWindowCommand ??= new RelayCommand(() =>
                {
                    AddUrlWindowViewModel viewModel = new (_urlModel);
                    AddUrlWindow auw = new(viewModel);
                    auw.ShowDialog();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
