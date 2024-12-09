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
        //private const int GWL_STYLE = -16;
        //private const int WS_SYSMENU = 0x80000;

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //[DllImport("user32.dll")]
        //private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public AddUrlWindowViewModel ViewModel { get; set; } = new ();

        public AddUrlWindow()
        {
            InitializeComponent();
            // Remove Window Control buttons from the title bar.
         //   var hwnd = new WindowInteropHelper(this).Handle;
         //var t =   SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
