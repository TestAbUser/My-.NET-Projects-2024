using DownloadManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.DataAccess
{
    public class SaveFileDialog: IOpenFileDialog
    {
        public string? Filter { get; set; }

        public string? FileName { get; set; }

        public bool? ShowDialog()
        {
            return new Microsoft.Win32.SaveFileDialog().ShowDialog();
        }
    }
}
