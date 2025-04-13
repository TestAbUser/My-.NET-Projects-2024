using DownloadManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

namespace DownloadManager.DataAccess
{
    public class SaveOrLoadFile : IFileSystem
    {
        public string[]? ReadFileLines(string? path)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));
            return File.ReadAllLines(path);
        }
        public void WriteLinesToFile(string? path, IEnumerable<string> lines)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));
            File.WriteAllLines(path, lines);
        }
    }
}
