using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.PresentationLogic
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>(Action? whenDone = null, object? model = null)
            where TViewModel : IViewModel;
    }
}
