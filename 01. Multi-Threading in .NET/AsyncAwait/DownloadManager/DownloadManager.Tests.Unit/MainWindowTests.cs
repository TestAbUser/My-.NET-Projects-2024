using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.DataAccess;
using DownloadManager.Domain;
using Moq;
using Xunit;
using System.Windows;
using DownloadManager.PresentationLogic;
using System.Collections.ObjectModel;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests : IDisposable
    {
        private Mock<IFileSystem> _persister;
        private IStringDownloader _strDownloader;
        private IPageRepository _pageRepo;

        // ObservableCollection<UrlModel> Urls = new();
        // AddUrlViewModel addWindow = new AddUrlViewModel(Urls);
        private Mock<IWindow> _window;
        private MainWindowViewModel _sut;

        public MainWindowTests()
        {
            _persister = new Mock<IFileSystem>();
            _strDownloader = new StringDownloader();
            _pageRepo = new DownloadedPageRepository(_strDownloader);
            _window = new Mock<IWindow>();
            _sut = new MainWindowViewModel(
               _pageRepo, _persister.Object, _window.Object);
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Load_urls_from_a_file()
        {
            _persister.Setup(x => x.LoadUrls()).Returns(["testUrl1"]);

            _sut.LoadUrlsCommand.Execute(null);

            Assert.Equal("testUrl1", _sut.Urls.First().Url);
            _persister.Verify(x => x.LoadUrls(), Times.Once);
        }

        [Fact]
        public void Change_progress_indicator()
        {

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
