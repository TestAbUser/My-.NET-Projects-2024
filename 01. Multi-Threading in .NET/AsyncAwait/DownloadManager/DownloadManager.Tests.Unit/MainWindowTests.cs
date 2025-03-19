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
using Xunit.Sdk;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using System.Windows.Navigation;

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

        // Create a helper class for setup.Ex: var stringCalculator = CreateDefaultStringCalculator();
        public MainWindowTests()
        {
            _loadFileDialog = new Mock<IOpenFileDialog>();
            _saveFileDialog = new Mock<IOpenFileDialog>();
            _fileSystem = new Mock<IFileSystem>();
            // _urlPersister = new UrlPersister(_loadFileDialog.Object,
            //     _saveFileDialog.Object, _fileSystem.Object);
            _urlPersister = new Mock<IUrlPersister>();
            _pageDownloader = new StringDownloader();
            _pageRepository = new Mock<IPageRepository>();//new PageRepository(_pageDownloader);
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
        public void Start_downloading_pages()
        {
            // arrange
            List<string> pages = new List<string>() { "test" };

            _pageRepository.Setup(x => x.DownloadPagesAsync(
                It.IsAny<string[]>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<(double, string)>?>()))
                .ReturnsAsync(pages);

            _sut = new MainWindowViewModel(
                _pageRepository.Object, _urlPersister.Object, _window.Object);

            // act
            _sut.DownloadCommand.Execute(null);

            // assert
            Assert.Equal("test", _sut.Pages.First());
        }

        [Fact]
        public async Task Cancel_downloading_pages()
        {
            var cont = TestContext.Current;
            // arrange
            var addresses = new string[] { "https://test1", "https://test2" };
            var prg = new Progress<(double, string)>();
            var cts = new CancellationTokenSource();
            // cts.Cancel();
            CancellationToken token = cts.Token;
            // token.ThrowIfCancellationRequested();
            //  IStringDownloader strDownloader = new StringDownloader();

            var stringDownloaderStub = new Mock<IStringDownloader>();
            //Task<string> setUp = Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //    return "test2";
            //});

            stringDownloaderStub.Setup(x =>
            x.DownloadPageAsStringAsync(
              It.IsAny<string>(),
              It.IsAny<CancellationToken>()))
                //.Throws(new OperationCanceledException())
                .ReturnsAsync("test", TimeSpan.FromMilliseconds(50));
              //  .Returns(setUp);
                //.Verifiable();

            var pageRepository = new PageRepository(stringDownloaderStub.Object);
            //  var res = pageRepository.DownloadPagesAsync(addresses, token,prg);


            _pageRepository.Setup(x => x.DownloadPagesAsync(
                It.IsAny<string[]>(),
                token,//It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<(double, string)>?>()))
                .ReturnsAsync(new List<string> { "test" },
                TimeSpan.FromMilliseconds(1000));

            // await strDownloader.DownloadPageAsStringAsync(addresses.First(),token);

            _sut = new MainWindowViewModel(
               pageRepository, _urlPersister.Object, _window.Object);
            _sut.Urls.Add(new UrlModel { Url = addresses.First() });
            _sut.Urls.Add(new UrlModel { Url = addresses.ElementAt(1) });

            //var res = pageRepository.DownloadPagesAsync(
            // addresses, token, prg);

            // act
            //_sut.CancellationToken = TestContext.Current.CancellationToken;
            // _sut.Cts = cts;
            // _sut.CancellationToken = token;

            var cans = _sut.DownloadCommand.ExecuteAsync(null);
            _sut.CancelCommand.Execute(null);

            //await Task.Run(() =>
            //{

            //    //    // await Task.Delay(500);
            //    //   await  _sut.DownloadCommand.ExecuteAsync(null);
            //    //    // await _sut.DownloadCommand.CanExecuteAsync(null);
            //    _sut.CancelCommand.Execute(null);

            //}, TestContext.Current.CancellationToken);

            //  _sut.CancelCommand.Execute(null);

            //var cans = Task.Run(() =>
            // _sut.CancelCommand.Execute(null),
            // TestContext.Current.CancellationToken);

            await cans;
            // res;


            // assert
            Assert.Equal("test", _sut.Pages.First());
            Assert.Equal("test", _sut.Pages.ElementAt(1));
            //await  Assert.ThrowsAsync<OperationCanceledException>(async()=>
            //     await pageRepository.DownloadPagesAsync(addresses,token, prg));
            // Assert.True(token.IsCancellationRequested);
            // stringDownloaderStub.Verify();
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
            //_pageRepository = new PageRepository(_pageDownloader);
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
    }
}
