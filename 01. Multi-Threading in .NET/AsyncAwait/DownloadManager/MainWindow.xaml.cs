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

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DownloadPages(object sender, RoutedEventArgs e)
        {
            string[] addresses = textBox.Text.Split(',');
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var t = Task.Run(async()=> await Downloader.Download(addresses, token));
            statBarText.Text="Downloading...";
            cancelBtn.IsEnabled = true;
            await t;
            cancelBtn.IsEnabled = false;
            statBarText.Text = "Ready";
        }

        private void CancelDownloading(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            cts.Dispose();
            cancelBtn.IsEnabled = false;
            statBarText.Text="Ready";
        }
    }
}