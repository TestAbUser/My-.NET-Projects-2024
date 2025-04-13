using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DownloadManager.PresentationLogic
{
    public static class PresentationCommands
    {
        private readonly static RoutedCommand _accept =
            new RoutedCommand("Accept", typeof(PresentationCommands));

        public static RoutedCommand Accept { get { return _accept; } }
    }
}
