﻿using System.Collections.ObjectModel;
using System.Windows;
using DownloadManager.PresentationLogic.Commands;
using DownloadManager.Domain;
using System.Windows.Input;

namespace DownloadManager.PresentationLogic.ViewModels
{
    // A view model for Add pop-up.
    public class AddUrlViewModel //: IViewModel
    {
        // private RelayCommand<object>? _okCommand;

        // Url property is bound to the Text property of the TextBox.
        public string Url { get; set; } = string.Empty;

        // ObservableCollection type notifies about changes in the collection.
        public ObservableCollection<UrlModel> Urls { get; }

        public AddUrlViewModel(ObservableCollection<UrlModel> Urls)
        {
            this.Urls = Urls;
        }

        internal void ClickOk()
        {
            Urls?.Add(new UrlModel { Url = Url, Status = "Ready" });
        }
    }
}
