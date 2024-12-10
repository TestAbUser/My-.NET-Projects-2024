using DownloadManager.ViewModels;
using System.Windows;


namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for AddUrlWindow.xaml
    /// </summary>
    public partial class AddUrlWindow : Window
    {
        public AddUrlWindowViewModel ViewModel { get; set; } = new ();

        public AddUrlWindow()
        {
            InitializeComponent();
        }
    }
}
