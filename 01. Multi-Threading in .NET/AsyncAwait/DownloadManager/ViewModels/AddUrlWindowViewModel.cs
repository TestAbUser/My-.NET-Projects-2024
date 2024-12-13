using System.Windows;
using DownloadManager.Commands;
using DownloadManager.Models;

namespace DownloadManager.ViewModels
{
    public class AddUrlWindowViewModel
    {
        private readonly UrlModel _urlModel;
        private RelayCommand<object> _okCommand;

        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; }

        public AddUrlWindowViewModel()
        {
        }
        public AddUrlWindowViewModel(UrlModel url)
        {
            _urlModel = url;
        }

        // When OK button is clicked addUrlWindow is passed as a CommandParameter to this property.
        public RelayCommand<object> OkCommand => _okCommand ??= new RelayCommand<object>(obj =>
        {
            _urlModel.Urls.Add(Url);

            // Casting the argument to Window. 
            Window wnd = obj as Window;
            wnd?.Close();
        }, null);
    }
}
