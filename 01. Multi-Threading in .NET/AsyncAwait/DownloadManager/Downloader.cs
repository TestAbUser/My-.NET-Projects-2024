//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace DownloadManager
//{
//    public class Downloader
//    {
//        public static HttpClient client = new HttpClient();

//        public static async Task Download(string[] addresses, CancellationToken ct)
//        {
//            string page;
//            await Task.Delay(10000);
//            try
//            {
//                foreach (string address in addresses)
//                {
//                    page = await client.GetStringAsync(address, ct);
//                }
//            }
//            catch (OperationCanceledException ex)
//            {
//                MessageBox.Show("Download cancelled");
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Something's wrong with the address!");
//            }
//            finally
//            {
//            }
//        }
//    }
//}
