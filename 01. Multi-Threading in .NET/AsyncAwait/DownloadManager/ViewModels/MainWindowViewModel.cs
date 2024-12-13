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
using System.Windows.Documents;

namespace DownloadManager.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<string> _urls;
        CancellationTokenSource cts;

        private readonly UrlModel _urlModel = new UrlModel();
        public UrlModel UModel { get; set; } //= new UrlModel();

        private RelayCommand _openAddWindowCommand = null;

        public ObservableCollection<UrlModel> UrlModels { get; } = new ObservableCollection<UrlModel>();
        //public ObservableCollection<string> Urls /*{ get;set; }*/
        //{
        //    get { return _urls; }
        //    set
        //    {
        //        _urls = value;
        //        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Urls)));
        //    }
        //}
        public MainWindowViewModel()
        {
           // UModel.Url = "test";
            //Urls= _urlModel.Urls;
           //UrlModels = UModel.Urls;
        }


        public RelayCommand OpenAddWindowCommand
        {
            get
            {
                return _openAddWindowCommand ??= new RelayCommand(() =>
                {
                    UrlModels.Add(UModel);
                    AddUrlWindowViewModel viewModel = new (UModel/*_urlModel*/);
                    AddUrlWindow auw = new(viewModel);
                    auw.ShowDialog();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
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
