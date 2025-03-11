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
       private readonly IPageRepository _repository;
        private readonly IUrlPersister _fileSystem;

        public MainViewModelFactory(IPageRepository repository, IUrlPersister fileSystem)
        {
            ArgumentNullException.ThrowIfNull(repository,nameof(repository));
            ArgumentNullException.ThrowIfNull(fileSystem,nameof(fileSystem));
            _repository = repository;
            _fileSystem = fileSystem;
            // _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public MainWindowViewModel Create(IWindow window)
        {
            ArgumentNullException.ThrowIfNull(window,nameof(window));

            return new MainWindowViewModel(_repository, _fileSystem, window);
        }
    }
}
