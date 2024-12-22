using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadManager.Models
{
    public class Downloader
    {
        private static readonly HttpClient s_client = new ();

        public static async Task<int> DownloadAsync(string[] addresses, CancellationToken ct, IProgress<int> progress=null)
        {
            string page;
            int totalCount = addresses.Length;
            int loadCount = await Task.Run<int>(async () =>
            {
                int tempCount = 1;
                    foreach (string address in addresses)
                    {
                    try
                    {
                        page = await s_client.GetStringAsync(address, ct).ConfigureAwait(false);
                        if (progress != null)
                        {
                            progress?.Report(tempCount * 100 / totalCount);
                        }
                        tempCount++;
                    } catch (OperationCanceledException ex) 
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                  
                    }
                return tempCount;
            }, ct);
            return loadCount;
        }
    }
}
