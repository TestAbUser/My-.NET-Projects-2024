﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Helpers
{
    public interface IStringDownloader
    {
        Task<string> DownloadPageAsStringAsync(string url, CancellationToken ct);
    }
}
