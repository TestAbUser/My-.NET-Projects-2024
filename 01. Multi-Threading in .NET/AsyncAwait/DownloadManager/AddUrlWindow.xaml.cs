using DownloadManager.ViewModels;
using DownloadManager.Models;
using System.Security.Policy;
using System.Windows;


namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for AddUrlWindow.xaml
    /// </summary>
    public partial class AddUrlWindow : Window
    {
        public AddUrlWindowViewModel ViewModel { get; set; }

        public AddUrlWindow()
        {
        }

        public AddUrlWindow(UrlModel url)
        {
            InitializeComponent();
            ViewModel = new(url);
        }
    }
}
