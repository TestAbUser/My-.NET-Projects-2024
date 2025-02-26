using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.PresentationLogic
{
    public interface IUrlManagementAgent
    {
        string[]? LoadUrls();
        void SaveUrls(IEnumerable<string> contents);
    }
}
