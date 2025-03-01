using DownloadManager.DataAccess;
using DownloadManager.Domain;
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
       private readonly IPageRepository _repo;
        private readonly IUrlPersister _persister;

        public MainViewModelFactory(IPageRepository repo, IUrlPersister persister)
        {
            ArgumentNullException.ThrowIfNull(repo,nameof(repo));
            ArgumentNullException.ThrowIfNull(persister,nameof(persister));
            _repo = repo;
            _persister = persister;
            // _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public MainWindowViewModel Create(IWindow window)
        {
            ArgumentNullException.ThrowIfNull(window,nameof(window));

            return new MainWindowViewModel(_repo, _persister, window);
        }
    }
}
