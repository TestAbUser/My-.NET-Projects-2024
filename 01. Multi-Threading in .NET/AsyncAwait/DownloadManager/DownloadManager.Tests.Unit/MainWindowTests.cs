using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadManager.Helpers;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.DataAccess;
using DownloadManager.Domain;
using Moq;
using Xunit;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests
    {
        [Fact]
        public void Load_urls_from_file()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.LoadFileContent())
                .Returns(["testUrl1", "testUrl2"]);
           // var sut = new MainWindowViewModel(fileSystemMock.Object);

           // sut.LoadUrlsCommand.Execute(null);
            // sut.SaveCommand.Execute(null);

            fileSystemMock.Verify(
                x => x.SaveDialog(new string[] { "testUrl1", "testUrl2" }),
                Times.Once);
        }

        [Fact]
        public void Save_urls_to_a_file()
        {
            
        }

        [Fact]
        public async Task Download_a_page_as_a_string()
        {
            string[] addresses = { "https://testing" };
            CancellationToken ct = CancellationToken.None;
            var downloaderMock = new Mock<IStringDownloader>();
            //downloaderMock.Setup(x => x.DownloadPageAsStringAsync("", ct))
            //    .Returns(()=>Task.FromResult("test"));
            downloaderMock.Setup(x => x.DownloadPageAsStringAsync("", ct))
               .Returns(async () =>
               {
                   await Task.Yield();
                   return "test";
               });
            var sut = new DownloadedPageRepository(downloaderMock.Object);

            var downloadedPages = await sut.DownloadAsync(addresses, ct);

            Assert.Equal("test", downloadedPages.ElementAt(0));
        }
    }
}
