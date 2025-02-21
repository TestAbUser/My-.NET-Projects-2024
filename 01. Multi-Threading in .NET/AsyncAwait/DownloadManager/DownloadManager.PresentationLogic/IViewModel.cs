using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.PresentationLogic
{
    public interface IViewModel
    {
        void Initialize(Action? whenDone, object? model);
    }
}
