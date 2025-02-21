using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Domain
{
    public interface IUrlRepository
    {
        string[]? LoadFileContent();
        void SaveDialog(IEnumerable<string> contents);
    }
}
