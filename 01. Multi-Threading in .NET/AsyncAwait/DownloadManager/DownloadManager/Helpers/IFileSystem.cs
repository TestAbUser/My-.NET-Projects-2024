using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Helpers
{
    public interface IFileSystem
    {
        string[]? LoadFileContent();
        void SaveDialog(IEnumerable<string> contents);
    }
}
