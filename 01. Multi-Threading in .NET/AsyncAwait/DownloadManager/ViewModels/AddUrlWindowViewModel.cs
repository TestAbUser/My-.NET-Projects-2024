using System.Runtime.InteropServices;
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
        public string Url
        {
            get => _url;
            set => _url = value;
        }

        private RelayCommand<object> _okCommand;

        // When OK button is clicked addPageAddressWindow is passed as a CommandParameter to this method.
        public RelayCommand<object> OkCommand => _okCommand ??= new RelayCommand<object>(obj=>
        {
            // Casting the argument to Window. 
            Window wnd = obj as Window;
            Urls.Add(Url);
            wnd?.Close();
        }, CanClose);

        private bool CanClose(object param) => true;





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
