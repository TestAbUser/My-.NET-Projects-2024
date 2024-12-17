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

        public static async Task<int> Download(string/*[]*/ address, IProgress<int> progress, CancellationToken ct)
        {
            string page;
            // int totalCount = address.Length;
            int loadCount = 0;//await Task.Run<int>(async () =>
           // {
                int tempCount = 0;

                //  await Task.Delay(10000);
                try
                {
                    //foreach (string address in addresses)
                    //{
                    /*page = */await client.GetStringAsync(address, ct);
                    Task.Delay(100);
                    if (progress != null)
                    {
                        // progress.Report(tempCount * 100 / totalCount);
                        progress.Report(100);
                    loadCount++;
                    }
                   // tempCount++;
                    //}
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show("Download cancelled");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something's wrong with the address!");
                    progress.Report(0);
                }
                finally
                {
                }
              //  return tempCount;
            //});
            return loadCount;
        }
    }
}
