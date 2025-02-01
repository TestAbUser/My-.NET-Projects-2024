using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadManager.Helpers;
using DownloadManager.ViewModels;
using Moq;
using Xunit;

namespace DownloadManager.Tests.Unit
{
    public class MainWindowTests
    {
        [Fact]
        public void Load_urls_from_file()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.LoadFileContent())
                .Returns(["testUrl1", "testUrl2"]);
            var sut = new MainWindowViewModel(fileSystemMock.Object);

            sut.OpenCommand.Execute(null);
            // sut.SaveCommand.Execute(null);

            fileSystemMock.Verify(
                x => x.SaveDialog(new string[] { "testUrl1", "testUrl2" }),
                Times.Once);
        }

        [Fact]
        public void Save_urls_to_a_file()
        {
            
        }
    }
}
