using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadManager.Models
{
    public class Downloader
    {
        private static readonly HttpClient s_client = new();

        public static async Task<int> DownloadAsync(string[] addresses, CancellationToken ct, IProgress<int> progress = null)
        {
            string page;
            int totalCount = addresses.Length;
            string res=null;
            int loadCount = await Task.Run<int>(async () =>
            {
                int tempCount = 1;
                using (var throttler = new SemaphoreSlim(1))
                {
                    IEnumerable<Task<string>> downloadPages = addresses.Select(address => Task.Run(async () =>
                         {
                             await throttler.WaitAsync();
                             try
                             {
                                  res = await s_client.GetStringAsync(address, ct).ConfigureAwait(false);
                                 progress?.Report(tempCount * 100 / totalCount);
                                 return res;
                             }
                             finally
                             {
                                 if (ct.IsCancellationRequested)
                                 {
                                     progress?.Report(-1);
                                 }
                                 else if (res==null && !ct.IsCancellationRequested)
                                 {
                                     progress?.Report(-2);
                                 }
                                 res=null;
                                 tempCount++;
                                 throttler.Release();
                                 // return res;
                             }
                         }));
                    try
                    {
                        string?[] pages = await Task.WhenAll(downloadPages).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return tempCount;
            }, ct).ConfigureAwait(false);
            return loadCount;
        }
    }
}
