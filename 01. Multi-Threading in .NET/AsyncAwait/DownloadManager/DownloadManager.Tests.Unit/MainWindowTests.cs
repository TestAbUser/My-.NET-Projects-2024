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
        public async Task Start_downloading_pages()
        {
            // arrange
            List<string> pages = new List<string>() { "test" };

            _pageRepository.Setup(x => x.DownloadPagesAsync(
                It.IsAny<string[]>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<(double, string)>?>()))
                .ReturnsAsync(pages, TimeSpan.FromMilliseconds(100));

            _sut = new MainWindowViewModel(
                _pageRepository.Object, _urlPersister.Object, _window.Object);

            // act
            _sut.DownloadCommand.Execute(null);
            await _sut.DownloadCommand.ExecutionTask!;

            // assert
            Assert.False(_sut.DownloadCommand.IsCancellationRequested);
            Assert.Equal("test", _sut.Pages.First());
        }

        [Fact]
        public async Task Cancel_downloading_pages()
        {
            // arrange
            var addresses = new string[] { "https://test1", "https://test2" };
            var stringDownloaderStub = new Mock<IStringDownloader>();

            stringDownloaderStub.Setup(x =>
            x.DownloadPageAsStringAsync(
              It.IsAny<string>(),
              It.IsAny<CancellationToken>()))
                .ReturnsAsync("test", TimeSpan.FromMilliseconds(1000));

            var pageRepository = new PageRepository(stringDownloaderStub.Object);

            _sut = new MainWindowViewModel(
               pageRepository, _urlPersister.Object, _window.Object);
            _sut.Urls.Add(new UrlModel { Url = addresses.First() });
            _sut.Urls.Add(new UrlModel { Url = addresses.ElementAt(1) });

            _sut.DownloadCommand.Execute(null);

            // act
            _sut.CancelCommand.Execute(null);
            await _sut.DownloadCommand.ExecutionTask!;

            // assert
            Assert.False(_sut.DownloadCommand.CanBeCanceled);
            Assert.True(_sut.DownloadCommand.IsCancellationRequested);

            Assert.Empty(_sut.Pages);
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

        //private MainWindowViewModel CreateMainWindowViewModel()
        //{
        //    return new MainWindowViewModel(this);
        //}

    }
}
