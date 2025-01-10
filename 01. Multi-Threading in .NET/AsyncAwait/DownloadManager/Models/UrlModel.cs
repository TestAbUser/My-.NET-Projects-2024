﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DownloadManager.Models
{
    public class UrlModel: INotifyPropertyChanged
    {
        private string _status=string.Empty;
        private string _url =string.Empty;
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
                OnPropertyChanged();
            }
        }

        public void AddUrl(string url)
        { 
            _urls.Add(url);
            OnPropertyChanged(nameof(AddUrl));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}
