
using DownloadManager.DataAccess;
using DownloadManager.Domain;
using DownloadManager.PresentationLogic;
using DownloadManager.PresentationLogic.ViewModels;
using Moq;
using System.Collections.ObjectModel;
using System.Security.Policy;
using Xunit;

namespace DownloadManager.Tests.Unit
{
    public class AddWindowTests
    {

        [Fact]
        public void Add_url_via_modal_window()
        {
            //var _persister = new Mock<IUrlPersister>();
            //var _strDownloader = new Mock<IStringDownloader>();
            //var _pageRepo = new Mock<IPageRepository>(_strDownloader);
            IFileSystem persister = new UrlPersister();
            IStringDownloader strDownloader = new StringDownloader();
            IPageRepository pageRepo = new DownloadedPageRepository(strDownloader);

            ObservableCollection<UrlModel> Urls = new();
            AddUrlViewModel addWindow = new AddUrlViewModel(Urls);
            var window = new Mock<IWindow>();
           //// _window.Setup(x => x.CreateChild(addWindow).ShowDialogue())
              //  .Returns(true);
            var sut = new MainWindowViewModel(pageRepo, persister, window.Object);

            // _sut.Urls.Add(new UrlModel { Url = "test", Status = "Ready" });

            sut.AddUrlCommand.CanExecute(true);
            sut.AddUrlCommand.Execute(null);
            window.Verify(x => x.CreateChild(addWindow).ShowDialogue(), 
                Times.Once);
           // Assert.Equal("test", _sut.Urls.First().Url);
        }

        [Fact]
        public void Adding_a_new_url()
        {
            //var _sut = new AddUrlViewModel { Url = "test" };

            //_sut.OkCommand.Execute(null);

            //Assert.Equal("test", _sut.Urls.Last().Url);
            //Assert.Equal("Ready", _sut.Urls.Last().Status);
        }

    }

}
