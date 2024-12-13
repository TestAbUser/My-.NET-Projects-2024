using System.Collections.ObjectModel;
using System.Windows;
using DownloadManager.Commands;
using DownloadManager.Models;

namespace DownloadManager.ViewModels
{
    public class AddUrlWindowViewModel
    {
        private readonly UrlModel _urlModel;
        private RelayCommand<object> _okCommand;
       // private string _url;
        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; }
        //{
        //    get => _url;
        //    set
        //    {
        //        _url = value;
        //        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Urls)));
        //    }
        //}
        public ObservableCollection<UrlModel> UModels { get; } = new ObservableCollection<UrlModel>();
        public AddUrlWindowViewModel()
        {
        }
        public AddUrlWindowViewModel(ObservableCollection<UrlModel> UrlModels)
        {
            // _urlModel = url;
            _urlModel = new UrlModel();
           UModels = UrlModels;
        }

        // When OK button is clicked addUrlWindow is passed as a CommandParameter to this property.
        public RelayCommand<object> OkCommand => _okCommand ??= new RelayCommand<object>(obj =>
        {
            //_urlModel.Urls.Add(Url);
            _urlModel.Url = Url;
            UModels.Add(_urlModel);
            // Casting the argument to Window. 
            Window wnd = obj as Window;
            wnd?.Close();
        }, null);
    }
}
