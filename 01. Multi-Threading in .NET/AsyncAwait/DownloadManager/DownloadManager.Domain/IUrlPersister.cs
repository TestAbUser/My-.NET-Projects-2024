using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Domain
{
    public interface IUrlPersister
    {
        void SaveUrls(IEnumerable<string> lines);
        string[]? LoadUrls();
    }
}