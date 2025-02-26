using DownloadManager.Domain;
using DownloadManager.PresentationLogic;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.DataAccess;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new DownloadManagerContainer();
            container.ResolveWindow().Show();
        }

        //public void NavigateTo<TViewModel>(Action? whenDone = null, object? model = null)
        //    where TViewModel : IViewModel
        //{
        //    Window window = CreateWindow(typeof(TViewModel));
        //    var viewModel = (IViewModel)window.DataContext;

        //    viewModel.Initialize(whenDone, model);
        //}

        //private Window CreateWindow(Type viewModelType)
        //{
        //    if (viewModelType == typeof(MainWindowViewModel))
        //    {
        //        return new MainWindow(new MainWindowViewModel(
        //            _urlRepo,
        //            _pageRepository,
        //            _navigationService));
        //    }

        //    else if (viewModelType == typeof(AddUrlViewModel))
        //    {
        //        return new AddUrlWindow(new AddUrlViewModel());
        //    }

        //    else
        //    {
        //        throw new InvalidOperationException($"Unknown view model: {viewModelType}.");
        //    }
        //}
    }

}










