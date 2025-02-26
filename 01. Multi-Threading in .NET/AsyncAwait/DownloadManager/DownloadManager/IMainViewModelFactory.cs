using DownloadManager.DataAccess;
using DownloadManager.PresentationLogic;
using DownloadManager.PresentationLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{
    public interface IMainViewModelFactory
    {
        MainWindowViewModel Create(IWindow window);
    }
}
