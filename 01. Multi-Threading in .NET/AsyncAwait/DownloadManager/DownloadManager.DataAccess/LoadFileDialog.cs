using DownloadManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DownloadManager.DataAccess
{
    public class LoadFileDialog : IOpenFileDialog
    {
        public string? Filter { get; set; }

        public string? FileName { get; set; }

        public bool? ShowDialog()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            bool? accept = dlg.ShowDialog();
            FileName = dlg.FileName;
            return accept;
        }
    }
}
