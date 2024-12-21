using System.Collections.ObjectModel;
using System.Windows;
using DownloadManager.Commands;
using DownloadManager.Models;

namespace DownloadManager.ViewModels
{
    public class AddUrlWindowViewModel
    {
        private RelayCommand<object> _okCommand;

        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; }

        public ObservableCollection<UrlModel> Urls { get; } = new();
        public AddUrlWindowViewModel()
        {
        }
        public AddUrlWindowViewModel(ObservableCollection<UrlModel> Urls)
        {
            this.Urls = Urls;
        }

        // When OK button is clicked addUrlWindow is passed as a CommandParameter to this property.
        public RelayCommand<object> OkCommand => _okCommand ??= new RelayCommand<object>(obj =>
        {
            Urls.Add(new UrlModel { Url = Url, Status = "Ready" });

            // Casting the argument to Window. 
            Window wnd = obj as Window;
            wnd?.Close();
        });
    }
}
