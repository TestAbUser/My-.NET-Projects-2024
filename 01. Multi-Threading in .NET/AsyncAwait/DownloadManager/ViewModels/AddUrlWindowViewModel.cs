using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows;
using DownloadManager.Commands;
using System.Collections.ObjectModel;

namespace DownloadManager.ViewModels
{
    public class AddUrlWindowViewModel
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private string _url; 
        public IList<string> Urls  = new ObservableCollection<string>();

        private RelayCommand _okCommand;

        public RelayCommand OkCommand
        {
            get;
        }


       

        //public AddUrlWindow()
        //{
        //    InitializeComponent();
        //}

        //private void OkBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    DialogResult = true;
        //}

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // Remove Window Control buttons from the title bar.
        //    var hwnd = new WindowInteropHelper(this).Handle;
        //    SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

        //    // As the window loads, move keyboard focus into the textbox.
        //    Keyboard.Focus(addWindowTextBox);
        //}
    }
}
