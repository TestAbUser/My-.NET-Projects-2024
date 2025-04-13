using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager.Domain
{
    public interface IPageRepository
    {
        Task<List<string>> DownloadPagesAsync(
            string[] addresses,
            CancellationToken ct,
            IProgress<ValueTuple<double, string>>? progress = null);
    }
}
