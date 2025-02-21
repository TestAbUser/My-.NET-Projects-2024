using DownloadManager.Domain;
using System.Net.Http;

namespace DownloadManager.DataAccess
{
    public class DownloadedPageRepository: IPageRepository
    {
        private readonly IStringDownloader _strDownloader;

        public DownloadedPageRepository()
        {
        }
        public DownloadedPageRepository(IStringDownloader strDownloader)
        {
            _strDownloader = strDownloader;
        }

        // Downloads pages as strings, allowing for cancellation and progress report.
        public async Task<List<string>> DownloadAsync(string[] addresses,
            CancellationToken ct, IProgress<ValueTuple<double, string>>? progress = null)
        {
            List<string> pages = [];
            int totalCount = addresses.Length;
            string page = string.Empty;

            int tempCount = 1;

            // Configure semaphor to hold only one thread to guarantee the order of downloading. 
            using var throttler = new SemaphoreSlim(1);

            // Using Select allows to launch all tasks concurrently (or would allow if more threads were involved).
            Task<string>[] downloadPages = addresses.Select(async address =>
            {
                await throttler.WaitAsync().ConfigureAwait(false);
                try
                {
                    // Using condition to check whether the url is valid, otherwise set its status as Failed. 
                    if (Uri.IsWellFormedUriString(address, UriKind.Absolute))
                    {
                        page = await _strDownloader.DownloadPageAsStringAsync(address, ct).ConfigureAwait(false);
                        progress?.Report(((double)tempCount * 100 / totalCount, "Completed"));
                        tempCount++;
                    }
                    else
                    {
                        progress?.Report(((double)tempCount * 100 / totalCount, "Failed"));
                    }
                    return page;
                }
                // Failed request isn't a reason to stop downloading the rest of the pages.
                catch (HttpRequestException)
                {
                    progress?.Report(((double)tempCount * 100 / totalCount, "Failed"));
                    return page;
                }
                finally
                {
                    page = string.Empty;
                    throttler.Release();
                }
            }).ToArray();
            try
            {
                pages = (await WhenAllOrError<string>(downloadPages)).ToList();
            }
            // All tasks will be cancelled, even completed.
            catch (OperationCanceledException) { progress?.Report((0, "Canceled")); }
            return pages;
        }
        /// <summary>
        /// Stops WhenAll task if there is an error in one of its tasks.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="tasks"></param>
        /// <returns>An array</returns>
        private static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
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
