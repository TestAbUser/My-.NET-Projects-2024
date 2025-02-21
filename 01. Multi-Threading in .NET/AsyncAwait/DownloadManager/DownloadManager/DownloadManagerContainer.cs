using DownloadManager.PresentationLogic;
using DownloadManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadManager
{
    public class DownloadManagerContainer: IDownloadManagerContainer
    {
        public IWindow ResolveWindow()
        {
            IUrlManagementAgent agent = new UrlManagementAgent();

            IMainViewModelFactory vmFactory = new MainViewModelFactory(agent);

            Window mainWindow = new MainWindow();
            IWindow window = new MainWindowAdapter(mainWindow, vmFactory);

            return window;
        }
    }
}
