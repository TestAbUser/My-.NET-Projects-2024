
namespace DownloadManager.Domain
{
    public interface IStringDownloader
    {
        Task<string> DownloadPageAsStringAsync(string url, CancellationToken ct);
    }
}
