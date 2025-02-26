using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DownloadManager.PresentationLogic;

namespace DownloadManager.DataAccess
{
    public class FileContent 
    {
        public readonly string[] Urls;

        public FileContent(string[] urls)
        {
            Urls = urls;
        }
    }
}
