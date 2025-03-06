
using DownloadManager.DataAccess;
using DownloadManager.Domain;
using DownloadManager.PresentationLogic.ViewModels;
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
            IUrlPersister persister = new UrlPersister();
            IStringDownloader strDownloader = new StringDownloader();
            IPageRepository pageRepo = new DownloadedPageRepository(strDownloader);
            var sut = new MainWindowViewModel(pageRepo, persister, null);
            sut.Urls.Add(new UrlModel { Url = "test", Status = "Ready" });

            sut.AddUrlCommand.CanExecute(true);
            sut.AddUrlCommand.Execute(null);

            Assert.Equal();
        }

        [Fact]
        public void Adding_a_new_url()
        {
            //var sut = new AddUrlViewModel { Url = "test" };

            //sut.OkCommand.Execute(null);

            //Assert.Equal("test", sut.Urls.Last().Url);
            //Assert.Equal("Ready", sut.Urls.Last().Status);
        }

    }

}
