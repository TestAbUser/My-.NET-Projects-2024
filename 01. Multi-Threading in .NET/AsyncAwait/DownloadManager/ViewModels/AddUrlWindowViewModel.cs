using System.Windows;
using System.Runtime.InteropServices;
using DownloadManager.Commands;
using DownloadManager.Models;

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

        private readonly UrlModel _urlModel;

        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; }

        public AddUrlWindowViewModel()
        {
        }
        public AddUrlWindowViewModel(UrlModel url)
        {
            _urlModel = url;
        }
        private RelayCommand<object> _okCommand;

        // When OK button is clicked addUrlWindow is passed as a CommandParameter to this method.
        public RelayCommand<object> OkCommand => _okCommand ??= new RelayCommand<object>(obj=>
        {
            _urlModel.Urls.Add(Url);

            // Casting the argument to Window. 
            Window wnd = obj as Window;
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
