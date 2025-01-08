using System.Net.Http;

namespace DownloadManager.Models
{
    public class Downloader
    {
        private static readonly HttpClient s_client = new();

        public static async Task<List<string>> DownloadAsync(string[] addresses,
            CancellationToken ct, IProgress<ValueTuple<double, string>>? progress = null)
        {
            List<string> pages = [];
            int totalCount = addresses.Length;
            string res = string.Empty;

            int tempCount = 1;
            using var throttler = new SemaphoreSlim(1);

            Task<string>[] downloadPages = addresses.Select(async address =>
            {
                await throttler.WaitAsync().ConfigureAwait(false);
                try
                {
                    if (Uri.IsWellFormedUriString(address, UriKind.Absolute))
                    {
                        res = await s_client.GetStringAsync(address, ct).ConfigureAwait(false);
                        progress?.Report(((double)tempCount * 100 / totalCount, "Completed"));
                        tempCount++;
                    }
                    else
                    {
                        progress?.Report(((double)tempCount * 100 / totalCount, "Failed"));
                    }
                    return res;
                }
                catch (HttpRequestException)
                {
                    progress?.Report(((double)tempCount * 100 / totalCount, "Failed"));
                    return res;
                }
                finally
                {
                    res = string.Empty;
                    throttler.Release();
                }
            }).ToArray();
            try
            {
                pages = (await WhenAllOrError<string>(downloadPages)).ToList();
            }
            catch (OperationCanceledException) { progress?.Report((0, "Canceled")); }
            return pages;
        }
        static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var killJoy = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks)
            {
                _ = task.ContinueWith(ant =>
                {
                    if (ant.IsCanceled)
                        killJoy.TrySetCanceled();
                    else if (ant.IsFaulted)
                        killJoy.TrySetException(ant.Exception.InnerExceptions);
                });
            }
            return await await Task.WhenAny(killJoy.Task, Task.WhenAll(tasks)).ConfigureAwait(false);
        }
    }
}
