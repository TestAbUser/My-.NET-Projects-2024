
using DownloadManager.Models;
using DownloadManager.ViewModels;
using System.Collections.ObjectModel;
using System.Security.Policy;
using Xunit;

namespace DownloadManager.Tests.Unit
{
    public class AddWindowTests
    {

        [Fact]
        public void Open_window_for_adding_urls()
        {
            var sut = new MainWindowViewModel();// { Urls {"test", "Ready" } };
            sut.Urls.Add(new UrlModel { Url ="test",Status ="Ready" });

            sut.OpenAddWindowCommand.CanExecute(true);
            sut.OpenAddWindowCommand.Execute(null);

           // Assert.Equal();
        }

        [Fact]
        public void Adding_a_new_url()
        {
            var sut = new AddUrlWindowViewModel { Url = "test" };

            sut.OkCommand.Execute(null);

            Assert.Equal("test", sut.Urls.Last().Url);
            Assert.Equal("Ready", sut.Urls.Last().Status);
        }

    }

}
