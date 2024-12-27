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
            string res = null;
            int loadCount = await Task.Run<int>(async () =>
            {
               // ct.ThrowIfCancellationRequested();
                int tempCount = 1;
                using (var throttler = new SemaphoreSlim(5))
                {

                    IEnumerable<Task<string>> downloadPages = addresses.Select(async (address) =>
                    // Task.Run(async () =>
                         {
                             // ct.ThrowIfCancellationRequested();
                             await throttler.WaitAsync().ConfigureAwait(false);
                             try
                             {
                                 /*res = await*/
                                 return await s_client.GetStringAsync(address).ConfigureAwait(false);
                                 //if (!ct.IsCancellationRequested)
                                 //    progress?.Report(tempCount * 100 / totalCount);
                                 //return res;
                             }
                             //catch (HttpRequestException ex)
                             //{
                             //   // return res;
                             //    //throw;
                             //}
                             finally
                             {
                                 //if (ct.IsCancellationRequested)
                                 //{
                                 //    progress?.Report(-1);
                                 //}
                                 //else if (res == null && !ct.IsCancellationRequested)
                                 //{
                                 //    progress?.Report(-2);
                                 //}
                                 //res = null;
                                 //tempCount++;
                                 throttler.Release();
                                 // return res;
                             }
                         });//.ToArray();
                    try
                    {
                        // var tasks= new Task[] {downloadPages}
                        string?[] pages = await Task.WhenAll(downloadPages).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException ex)//(Exception ex) when(ex is not OperationCanceledException)
                    {

                    }
                }

                return tempCount;
            }).ConfigureAwait(false);
            return loadCount;
        }
    }
}
