
using DownloadManager.Models;
using DownloadManager.ViewModels;
using System.Collections.ObjectModel;
using Xunit;

namespace DownloadManager.Tests.Unit
{
    public class AddWindowTests
    {
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
