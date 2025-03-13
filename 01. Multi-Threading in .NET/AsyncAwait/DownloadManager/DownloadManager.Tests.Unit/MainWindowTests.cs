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
using System.Security.Policy;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests : IDisposable
    {
        private Mock<IOpenFileDialog> _loadFileDialog;
        private Mock<IOpenFileDialog> _saveFileDialog;
        private Mock<IFileSystem> _fileSystem;

        private Mock<IUrlPersister> _urlPersister;
        private IStringDownloader _pageDownloader;
        private Mock<IPageRepository> _pageRepository;

        // ObservableCollection<UrlModel> Urls = new();
        // AddUrlViewModel addWindow = new AddUrlViewModel(Urls);
        private Mock<IWindow> _window;
        private MainWindowViewModel _sut;

        public MainWindowTests()
        {
            _loadFileDialog = new Mock<IOpenFileDialog>();
            _saveFileDialog = new Mock<IOpenFileDialog>();
            _fileSystem = new Mock<IFileSystem>();
            // _urlPersister = new UrlPersister(_loadFileDialog.Object,
            //     _saveFileDialog.Object, _fileSystem.Object);
            _urlPersister = new Mock<IUrlPersister>();
            _pageDownloader = new StringDownloader();
            _pageRepository = new Mock<IPageRepository>();//new DownloadedPageRepository(_pageDownloader);
            _window = new Mock<IWindow>();
            //_sut = new MainWindowViewModel(
            //   _pageRepository, _urlPersister, _window.Object);
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Load_urls_from_a_file()
        {
            // arrange
            // var urlPersister = new Mock<IUrlPersister>();
            _urlPersister.Setup(x => x.LoadUrls()).Returns(["testUrl1"]);
            _sut = new MainWindowViewModel(_pageRepository.Object,
                _urlPersister.Object, _window.Object);

            // act
            _sut.LoadUrlsCommand.Execute(null);

            // assert
            Assert.Equal("testUrl1", _sut.Urls.First().Url);
            // _urlPersister.Verify(x => x.LoadUrls(), Times.Once);
        }

        [Fact]
        public void Change_progress_indicator()
        {

        }

        [Fact]
        public void Save_urls_to_a_file()
        {
            // arrange
            // var urlPersister = new Mock<IUrlPersister>();

            _sut = new MainWindowViewModel(
                _pageRepository.Object, _urlPersister.Object, _window.Object);
            _sut.Urls.Add(new UrlModel { Url = "test" });

            // act
            _sut.SaveCommand.Execute(null);

            // assert
            _urlPersister.Verify(x =>
                   x.SaveUrlsToFile(new string[] { "test" }), Times.Once);
        }

        [Fact]
        public void Click_add_button_to_open_modal_window()
        {

        }

        [Fact]
        public void Add_a_new_url()
        {
            // arrange
            ObservableCollection<UrlModel> testUrlModels = 
                                       [new UrlModel { Url = "Url1" }];
            AddUrlViewModel auvm = new(testUrlModels);
            _window.Setup(x => 
            x.CreateChild(auvm).ShowDialogue()).Returns(true);

            _sut = new(
                _pageRepository.Object, _urlPersister.Object, _window.Object);
            auvm.Url = "Url2";

            // act
            _sut.AddUrlCommand.Execute(auvm);

            // assert
            Assert.Equal("Url2", testUrlModels.Last().Url);
            Assert.Equal("Ready", testUrlModels.Last().Status);
            Assert.Equal(2, testUrlModels.Count);
        }

        [Fact]
        public void Save_urls_to_a_file_Copy()
        {
            // arrange
            var _loadFileDialog = new LoadFileDialog();//new Mock<IOpenFileDialog>();
            var _saveFileDialog = new Mock<IOpenFileDialog>();
            _fileSystem = new Mock<IFileSystem>();

            _saveFileDialog.SetupGet(x => x.FileName).Returns("testName");
            _saveFileDialog.Setup(x => x.ShowDialog()).Returns(true);

            // _fileSystem.Setup(x => x.ReadFileLines("testName")).
            //     Returns(new[] { "testValue1" });

            //        _urlPersister = new UrlPersister(_loadFileDialog,
            //_saveFileDialog.Object, _fileSystem.Object);
            // _urlPersister = new Mock<IUrlPersister>();
            //_pageDownloader = new StringDownloader();
            //_pageRepository = new DownloadedPageRepository(_pageDownloader);
            //_window = new Mock<IWindow>();
            //_sut = new MainWindowViewModel(
            //   _pageRepository, _urlPersister, _window.Object);


            //_urlPersister = new UrlPersister(
            //    _loadFileDialog.Object,
            //    _saveFileDialog.Object,
            //    _fileSystem.Object);
            string[] urls = ["testUrl"];
            // _urlPersister.SaveUrlsToFile(urls);

            // act
            // _sut.SaveCommand.Execute(null);

            // assert
            _fileSystem.Verify(x => x.WriteLinesToFile(
                "testName", new string[] { "testUrl" }),
                Times.Once);
            // _urlPersister.Verify(x => x.SaveUrlsToFile(), Times.Once);
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
