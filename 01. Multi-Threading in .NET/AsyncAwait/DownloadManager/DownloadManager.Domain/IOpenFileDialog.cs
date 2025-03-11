using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Domain
{
    public interface IOpenFileDialog
    {
        string? Filter { get; set; }
        bool? ShowDialog();
        string? FileName { get; set; }
    }
}
