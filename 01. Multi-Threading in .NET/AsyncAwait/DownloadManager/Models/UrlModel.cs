using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DownloadManager.Models
{
    public class UrlModel: INotifyPropertyChanged
    {
        private string _status;
        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
               // PropertyChanged(this, new PropertyChangedEventArgs(nameof(Url)));
            }
        }
        private readonly ObservableCollection<string> _urls = new();

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
            }
        }

        private bool _isChanged;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (value == _isChanged) return;
                _isChanged = value;
                OnPropertyChanged();
            }
        }

        public UrlModel()
        {
            //Urls = new ObservableCollection<string>(_urls);
        }

        public void AddUrl(string url)
        { 
            _urls.Add(url);
            OnPropertyChanged(nameof(AddUrl));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            if (propertyName != nameof(IsChanged))
            {
                IsChanged = true;
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
