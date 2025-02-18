using DownloadManager.ViewModels;
using System.Windows;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for AddUrlWindow.xaml
    /// </summary>
    public partial class AddUrlWindow : Window
    {
        public AddUrlWindow()
        {
            InitializeComponent();
        }

        public AddUrlWindow(AddUrlWindowViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
