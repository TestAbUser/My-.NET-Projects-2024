using System.Collections.ObjectModel;
using System.Windows;
using DownloadManager.PresentationLogic.Commands;
using DownloadManager.Domain;
using System.Windows.Input;

namespace DownloadManager.PresentationLogic.ViewModels
{
    // A view model for Add pop-up.
    public class AddUrlViewModel : IViewModel
    {
       // private RelayCommand<object>? _okCommand;

        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; } = string.Empty;

        // ObservableCollection type notifies about changes in the collection.
        public ObservableCollection<UrlModel> Urls { get; } = new();

        public AddUrlViewModel(ObservableCollection<UrlModel> Urls)
        {
            this.Urls = Urls;
        }
        //public ICommand OkCommand =>
        //    _okCommand ??= new RelayCommand<object>(OkClicked);

        // When OK button is clicked addUrlWindow is passed as a CommandParameter to this property.
        //public RelayCommand<object> OkCommand => 
        //    _okCommand ??= new RelayCommand<object>(obj =>
        //             {
        //            Urls?.Add(new UrlModel { Url = Url, Status = "Ready" });

        //            // Casting the argument to Window. 
        //            Window? wnd = obj as Window;
        //    wnd?.Close();
        //});

        private void OkClicked(object param)
        {
            Urls?.Add(new UrlModel { Url = Url, Status = "Ready" });

            // Casting the argument to Window. 
           // Window? wnd = obj as Window;
           // wnd?.Close();
        }

        public void Initialize(Action? action, object? model)
        {

        }
    }
}
