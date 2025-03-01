using DownloadManager.PresentationLogic;
using DownloadManager.DataAccess;
using System.Windows;
using DownloadManager.Domain;

namespace DownloadManager
{
    public class DownloadManagerContainer : IDownloadManagerContainer
    {
        public IWindow ResolveWindow()
        {
            IUrlPersister persister = new UrlPersister();
            IStringDownloader strDownloader = new StringDownloader();
            IPageRepository pageRepo = new DownloadedPageRepository(strDownloader);

            IMainViewModelFactory vmFactory = new MainViewModelFactory(pageRepo, persister);

            Window mainWindow = new MainWindow();
            IWindow window = new MainWindowAdapter(mainWindow, vmFactory);

            return window;
        }
    }
}
