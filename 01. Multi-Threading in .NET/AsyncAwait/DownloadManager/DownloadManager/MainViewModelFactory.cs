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
        private readonly IUrlManagementAgent _agent;

        public MainViewModelFactory(IUrlManagementAgent agent)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public MainViewModel Create(IWindow window)
        {
            ArgumentNullException.ThrowIfNull(window,nameof(window));

            return new MainViewModel(window);
        }
    }
}
