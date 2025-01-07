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

                int tempCount = 1;
                using var throttler = new SemaphoreSlim(1);

                    List<Task<string?>> downloadPages = addresses.Select(async (address, ct) =>
                         {
                             // ct.ThrowIfCancellationRequested();
                             await throttler.WaitAsync().ConfigureAwait(false);
                             try
                             {
                                 if(Uri.IsWellFormedUriString(address,UriKind.Absolute))
                                 res = await s_client.GetStringAsync(address).ConfigureAwait(false);
                                else progress?.Report(-2);
                                 //if (!ct.IsCancellationRequested)
                                 // progress?.Report(tempCount * 100 / totalCount);
                                 return res;
                             }
                             catch (HttpRequestException ex) //when (ex is not OperationCanceledException)
                             {
                                 progress?.Report(-2);
                                  return res;
                                 //throw;
                             }
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
                                 tempCount++;
                                 throttler.Release();
                                 // return res;
                             }
                         }).ToList();
                    while(downloadPages.Count()>0)
                    {
                        try
                        {

                            var task = await Task.WhenAny(downloadPages);
                            downloadPages.Remove(task);
                            await task;
                        }
                        catch { }
                    }
                    //try
                    //{
                    //    // var tasks= new Task[] {downloadPages}
                    //    /*string?[]*/
                    //    //var task = await Task.WhenAll(downloadPages).ConfigureAwait(false);
                    //    //var st = task.Status;
                    //    // pages = await task;
                    //}
                    //catch (Exception ex)//(Exception ex) when(ex is not OperationCanceledException)
                    //{
                    ////    //foreach (Task faulted in downloadPages.Where(t => t.IsFaulted))
                    ////    //{
                    ////    //   // progress?.Report(-2);
                    ////    //}
                    //}
                //}

                return tempCount;

        }
    }
}
