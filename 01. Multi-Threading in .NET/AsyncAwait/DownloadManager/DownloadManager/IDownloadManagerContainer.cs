﻿using DownloadManager.PresentationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{
    public interface IDownloadManagerContainer
    {
        IWindow ResolveWindow();
    }
}
