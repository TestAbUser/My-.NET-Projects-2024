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
using System.Runtime.CompilerServices;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests : IDisposable
    {
        private readonly Mock<IOpenFileDialog> _loadFileDialog;
        private readonly Mock<IOpenFileDialog> _saveFileDialog;
        private readonly Mock<IFileSystem> _fileSystem;

        private readonly Mock<IUrlPersister> _urlPersister;
        private readonly IStringDownloader _pageDownloader;
        private readonly Mock<IPageRepository> _pageRepository;

        // ObservableCollection<UrlModel> Urls = new();
        // AddUrlViewModel addWindow = new AddUrlViewModel(Urls);
        private readonly Mock<IWindow> _window;
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
            //   pageRepository, _urlPersister, window.Object);
        }

        public void Dispose()
        {

        }

        [Theory]
        [InlineData([new string[] { "https://test1", "https://test2" }])]
        [InlineData([new string[] { }])]
        public void Load_urls_from_a_file(string[] urls)
        {
            // arrange
            _urlPersister.Setup(x => x.LoadUrls()).Returns(urls);
            _sut = new MainWindowViewModel(_pageRepository.Object,
                _urlPersister.Object, _window.Object);

            // act
            _sut.LoadUrlsCommand.Execute(null);

            // assert

            Assert.Equal(urls, _sut.Urls.Select(x => x.Url));
            _urlPersister.Verify(x => x.LoadUrls(), Times.Once);
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
            List<string> pages = ["test"];
            Mock<IPageRepository> pageRepository = new();
            Mock<IUrlPersister> urlPersister = new();
            Mock<IWindow> window = new();

            pageRepository.Setup(x => x.DownloadPagesAsync(
                It.IsAny<string[]>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<(double, string)>?>()))
                .ReturnsAsync(pages, TimeSpan.FromMilliseconds(100));

            var sut = CreateMainWindowViewModel(
                 pageRepository.Object, urlPersister.Object, window.Object);

            // act
            sut.DownloadCommand.Execute(null);
            await sut.DownloadCommand.ExecutionTask!;

            // assert
            Assert.False(sut.DownloadCommand.IsCancellationRequested);
            Assert.Equal("test", sut.Pages[0]);
        }

        [Fact]
        public async Task Cancel_downloading_pages()
        {
            // arrange
            Mock<IUrlPersister> urlPersister = new();
            Mock<IWindow> window = new();
            string[] addresses = { "https://test1", "https://test2" };
            Mock<IStringDownloader> stringDownloaderStub = new();

            stringDownloaderStub.Setup(x =>
            x.DownloadPageAsStringAsync(
              It.IsAny<string>(),
              It.IsAny<CancellationToken>()))
                .ReturnsAsync("test", TimeSpan.FromMilliseconds(100));

            PageRepository pageRepository = new(stringDownloaderStub.Object);

            MainWindowViewModel sut = CreateMainWindowViewModel(
               pageRepository, urlPersister.Object, window.Object);

            sut.Urls.Add(new UrlModel { Url = addresses[0] });
            sut.Urls.Add(new UrlModel { Url = addresses[1] });

            sut.DownloadCommand.Execute(null);

            // act
            sut.CancelCommand.Execute(null);
            await sut.DownloadCommand.ExecutionTask!;

            // assert
            Assert.False(sut.DownloadCommand.CanBeCanceled);
            Assert.True(sut.DownloadCommand.IsCancellationRequested);

            Assert.Empty(sut.Pages);
        }

        [Fact]
        public void Add_a_new_url()
        {
            // arrange
            Mock<IPageRepository> pageRepository = new();
            Mock<IUrlPersister> urlPersister = new();
            Mock<IWindow> window = new();

            ObservableCollection<UrlModel> testUrlModels =
                                           [new UrlModel { Url = "Url1" }];
            AddUrlViewModel auvm = new(testUrlModels);
            window.Setup(x => x.CreateChild(auvm)
                               .ShowDialogue())
                               .Returns(true);

            var sut = CreateMainWindowViewModel(pageRepository.Object,
                urlPersister.Object, window.Object);
            auvm.Url = "Url2";

            // act
            sut.AddUrlCommand.Execute(auvm);

            // assert
            Assert.Equal("Url2", testUrlModels.Last().Url);
            Assert.Equal("Ready", testUrlModels.Last().Status);
            Assert.Equal(2, testUrlModels.Count);
        }

        private static MainWindowViewModel CreateMainWindowViewModel(
       IPageRepository pageRepository,
       IUrlPersister urlPersister,
       IWindow window)
        {
            return new MainWindowViewModel(pageRepository, urlPersister, window);
        }

    }
}
