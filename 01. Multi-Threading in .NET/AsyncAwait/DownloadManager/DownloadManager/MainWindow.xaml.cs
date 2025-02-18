using System.Windows;
using DownloadManager.Helpers;
using DownloadManager.ViewModels;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel(new FileSystem());
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}