using DownloadManager.ViewModels;
using System.Windows;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for AddUrlWindow.xaml
    /// </summary>
    public partial class AddUrlWindow : Window
    {
        public AddUrlWindow(AddUrlWindowViewModel viewModel) 
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
