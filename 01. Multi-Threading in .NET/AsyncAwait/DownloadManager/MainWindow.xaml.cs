using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DownloadManager;
using DownloadManager.ViewModels;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      //  public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();
      //  CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        //private async void DownloadPages(object sender, RoutedEventArgs e)
        //{
        //    string[] addresses = addWindowTextBox.Text.Split(','); 
        //    cts = new CancellationTokenSource();
        //    CancellationToken token = cts.Token;
        //    var t = Task.Run(async () => await Downloader.Download(addresses, token));
        //    statBarText.Text = "Downloading...";
        //    downloadBtn.IsEnabled = false;
        //    cancelBtn.IsEnabled = true;
        //    await t;
        //    await Downloader.Download(addresses, token);
        //    cancelBtn.IsEnabled = false;
        //    downloadBtn.IsEnabled = true;
        //    statBarText.Text = "Ready";
        //}

        //private void CancelDownloading(object sender, RoutedEventArgs e)
        //{
        //    cts.Cancel();
        //    cts.Dispose();
        //    cancelBtn.IsEnabled = false;
        //    downloadBtn.IsEnabled = true;
        //    statBarText.Text="Ready";
        //}



        //private void AddUrlBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    AddUrlWindow auw = new();
        //    auw.ShowDialog();
        //}
    }
}