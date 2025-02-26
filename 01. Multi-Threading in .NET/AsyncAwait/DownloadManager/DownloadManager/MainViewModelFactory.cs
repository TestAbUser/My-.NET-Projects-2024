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
    public class MainViewModelFactory: IMainViewModelFactory
    {
       // private readonly IUrlManagementAgent _agent;
      // private readonly IEnumerable<string> _urls;
        private readonly Persister _persister;

        public MainViewModelFactory(Persister persister)//IUrlManagementAgent agent)
        {
            //ArgumentNullException.ThrowIfNull(urls,nameof(urls));
            ArgumentNullException.ThrowIfNull(persister,nameof(persister));
           // _urls = urls;
            _persister = persister;
            // _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public MainWindowViewModel Create(IWindow window)
        {
            ArgumentNullException.ThrowIfNull(window,nameof(window));

            return new MainWindowViewModel(_persister,window);
        }
    }
}
