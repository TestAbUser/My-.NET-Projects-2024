using System.Net.Http;
using System.Windows.Documents;


namespace DownloadManager.Models
{
    public class Downloader
    {
        private static readonly HttpClient s_client = new();

        public static async Task<List<string>> DownloadAsync(string[] addresses, CancellationToken ct, IProgress<int> progress = null)
        {
            List<string> pages = [];
            int totalCount = addresses.Length;
            string res = string.Empty;

            int tempCount = 1;
            using var throttler = new SemaphoreSlim(1);

            IEnumerable<Task<string>> downloadPages = addresses.Select(async address =>
                 {
                     await throttler.WaitAsync().ConfigureAwait(false);
                     try
                     {
                         if (!ct.IsCancellationRequested)
                         {
                             if (Uri.IsWellFormedUriString(address, UriKind.Absolute))
                             {
                                 res = await s_client.GetStringAsync(address).ConfigureAwait(false);
                                 progress?.Report(tempCount * 100 / totalCount);
                             }
                             else progress?.Report(-2);
                             //if (!ct.IsCancellationRequested)
                             // return res;
                         }
                         else
                         {
                             progress?.Report(-1);
                         }
                         return res;
                     }
                     catch (HttpRequestException ex)
                     {
                         progress?.Report(-2);
                         return res;
                         throw;
                     }

                     finally
                     {
                         tempCount++;
                         throttler.Release();
                         res = string.Empty;
                     }
                 });

            pages = (await Task.WhenAll(downloadPages).ConfigureAwait(false)).ToList();

            return pages;
        }
    }
}
