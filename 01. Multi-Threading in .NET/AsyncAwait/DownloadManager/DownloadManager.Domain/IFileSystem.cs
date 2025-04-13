using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Domain
{
    public interface IFileSystem
    {
        string[]? ReadFileLines(string? path);
        void WriteLinesToFile(string? path, IEnumerable<string> lines);
    }
}
