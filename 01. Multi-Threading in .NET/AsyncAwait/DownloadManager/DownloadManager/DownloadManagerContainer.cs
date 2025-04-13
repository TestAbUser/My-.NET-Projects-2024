using DownloadManager.PresentationLogic;
using DownloadManager.DataAccess;
using System.Windows;
using DownloadManager.Domain;
//using Microsoft.Win32;

namespace DownloadManager
{
    public class DownloadManagerContainer : IDownloadManagerContainer
    {
        public IWindow ResolveWindow()
        {
            IOpenFileDialog loadFileDialog = new LoadFileDialog();
            IOpenFileDialog saveFileDialog = new SaveFileDialog();
            IFileSystem file = new SaveOrLoadFile();
            IUrlPersister fileSystem = new UrlPersister(loadFileDialog, saveFileDialog, file);
            IStringDownloader strDownloader = new StringDownloader();
            IPageRepository pageRepo = new PageRepository(strDownloader);

            IMainViewModelFactory vmFactory = new MainViewModelFactory(pageRepo, fileSystem);

            Window mainWindow = new MainWindow();
            IWindow window = new MainWindowAdapter(mainWindow, vmFactory);

            return window;
        }
    }
}
