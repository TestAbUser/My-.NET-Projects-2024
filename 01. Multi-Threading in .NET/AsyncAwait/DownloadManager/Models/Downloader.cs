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
    public class Downloader//: INotifyPropertyChanged
    {
        public static HttpClient client = new HttpClient();

        public static async Task<int> Download(string[] addresses, IProgress<int> progress, CancellationToken ct)
        {
            string page;
            int totalCount = addresses.Length;
            int loadCount = await Task.Run<int>(async () =>
            {
                int tempCount = 1;

                 // await Task.Delay(1000);
                try
                {
                    foreach (string address in addresses)
                    {
                        page = await client.GetStringAsync(address, ct);
                        if (progress != null)
                        {
                            progress.Report(tempCount * 100 / totalCount);
                        }
                        tempCount++;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show("Download cancelled");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something's wrong with the address!");
                }
                finally
                {
                }
                return tempCount;
            });
            return loadCount;
        }
    }
}
