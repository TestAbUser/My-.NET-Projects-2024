using DownloadManager.PresentationLogic.ViewModels;
using DownloadManager.DataAccess;
using DownloadManager.Domain;
using Moq;
using Xunit;
using DownloadManager.PresentationLogic;
using System.Collections.ObjectModel;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests
    {
        [Theory]
        [InlineData([new string[] { "https://test1", "https://test2" }])]
        [InlineData([new string[] { }])]
        public void Load_urls_from_a_file(string[] urls)
        {
            // arrange
            (var pageRepository, var urlPersister, var window) =
                           InitializeMainWindowViewModelParameters();
            urlPersister.Setup(x => x.LoadUrls()).Returns(urls);

            MainWindowViewModel sut = CreateMainWindowViewModel(pageRepository.Object,
             urlPersister.Object, window.Object);

            // act
            sut.LoadUrlsCommand.Execute(null);

            // assert
            Assert.Equal(urls, sut.Urls.Select(x => x.Url));
            urlPersister.Verify(x => x.LoadUrls(), Times.Once);
        }

        [Fact]
        public void Change_progress_indicator()
        {

        }

        [Fact]
        public void Save_urls_to_a_file()
        {
            // arrange
            (var pageRepository, var urlPersister, var window) =
                         InitializeMainWindowViewModelParameters();

            MainWindowViewModel sut = CreateMainWindowViewModel(
                pageRepository.Object, urlPersister.Object, window.Object);
            sut.Urls.Add(new UrlModel { Url = "test" });

            // act
            sut.SaveCommand.Execute(null);

            // assert
            urlPersister.Verify(x =>
                   x.SaveUrlsToFile(new string[] { "test" }), Times.Once);
        }

        [Fact]
        public async Task Start_downloading_pages()
        {
            // arrange
            List<string> pages = ["test"];
            (var pageRepository, var urlPersister, var window) =
                    InitializeMainWindowViewModelParameters();

            pageRepository.Setup(x => x.DownloadPagesAsync(
                It.IsAny<string[]>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<IProgress<(double, string)>?>()))
                .ReturnsAsync(pages, TimeSpan.FromMilliseconds(100));

            MainWindowViewModel sut = CreateMainWindowViewModel(
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
            (var pageRepository, var urlPersister, var window) =
                 InitializeMainWindowViewModelParameters();


            ObservableCollection<UrlModel> testUrlModels =
                                           [new UrlModel { Url = "Url1" }];
            AddUrlViewModel auvm = new(testUrlModels);
            window.Setup(x => x.CreateChild(auvm)
                               .ShowDialogue())
                               .Returns(true);

            MainWindowViewModel sut = CreateMainWindowViewModel(pageRepository.Object,
                                                                urlPersister.Object,
                                                                window.Object);
            auvm.Url = "Url2";

            // act
            sut.AddUrlCommand.Execute(auvm);

            // assert
            Assert.Equal("Url2", testUrlModels.Last().Url);
            Assert.Equal("Ready", testUrlModels.Last().Status);
            Assert.Equal(2, testUrlModels.Count);
        }

        // Using Factory method instead of constructor to initialize SUT makes
        // tests more readable and avoids shared data between tests (Khorikov)
        private static MainWindowViewModel CreateMainWindowViewModel(
       IPageRepository pageRepository,
       IUrlPersister urlPersister,
       IWindow window)
        {
            return new MainWindowViewModel(pageRepository, urlPersister, window);
        }

        private static (Mock<IPageRepository>, Mock<IUrlPersister>, Mock<IWindow>) InitializeMainWindowViewModelParameters()
        {
            Mock<IPageRepository> pageRepository = new();
            Mock<IUrlPersister> urlPersister = new();
            Mock<IWindow> window = new();
            return (pageRepository, urlPersister, window);
        }

    }
}
