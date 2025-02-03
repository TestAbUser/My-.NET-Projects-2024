using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Helpers
{
    internal class StringDownloader: IStringDownloader
    {
        private static readonly HttpClient s_client = new();

        public async Task<string> DownloadPageAsStringAsync(string url, CancellationToken ct)
        {
           return await s_client.GetStringAsync(url, ct);
        }
    }
}
