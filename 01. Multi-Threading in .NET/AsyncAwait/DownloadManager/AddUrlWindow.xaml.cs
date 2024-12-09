using DownloadManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            // As the window loads, move keyboard focus into the textbox.
          //  Keyboard.Focus(addWindowTextBox);
        }
    }
}
