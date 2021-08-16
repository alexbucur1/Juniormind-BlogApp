using BlogApp.Dotnet.ApplicationCore.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Services.Tests
{
    public class ImageServiceFacts
    {
        [Fact]
        public async Task UploadImage_CallsCopyTo_IfFileDoesntExist_ReturnsImageURL()
        {
            int id = 1;

            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<IFileSystem>();
            var mockIFile = new Mock<IFile>();
            var mockFileStream = new MemoryStream();

            mockIFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockFileSystem.SetupGet(x => x.File).Returns(mockIFile.Object);

            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.SetupGet(x => x.FileName).Returns("1.jpg");
            mockIFormFile.SetupGet(x => x.Length).Returns(15);
            mockIFormFile.Setup(x => x.CopyTo(mockFileStream))
                    .Callback<Stream>(l => l.CopyTo(mockFileStream));

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.UploadImage(mockIFormFile.Object, id);

            mockIFormFile.Verify(mock => mock.CopyTo(It.IsAny<Stream>()), Times.Once());
            Assert.Equal("/Assets/Uploads/1.jpg", result);
        }

        [Fact]
        public async Task UploadImage_DoesNotCallCopyTo_IfFileExists_ReturnsOldImageURL()
        {
            int id = 1;

            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<IFileSystem>();
            var mockIFile = new Mock<IFile>();
            var mockFileStream = new MemoryStream();

            mockIFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.SetupGet(x => x.File).Returns(mockIFile.Object);

            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.SetupGet(x => x.FileName).Returns("1.jpg");
            mockIFormFile.SetupGet(x => x.Length).Returns(15);
            mockIFormFile.Setup(x => x.CopyTo(mockFileStream))
                    .Callback<Stream>(l => l.CopyTo(mockFileStream));

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.UploadImage(mockIFormFile.Object, id);

            mockIFormFile.Verify(mock => mock.CopyTo(It.IsAny<Stream>()), Times.Never());
            Assert.Equal("/Assets/Uploads/1.jpg", result);
        }

        [Fact]
        public async Task UploadImage_ReturnsEmptyString_InvalidExtension()
        {
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<FileSystem>();
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 1, 15, "Data", "dummy.exe");

            int id = 5;

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.UploadImage(file, id);
            Assert.Equal("", result);
        }

        [Fact]
        public async Task UploadImage_ReturnsEmptyString_ImageFileLengthIs0()
        {
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<FileSystem>();
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.jpg");

            int id = 5;

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.UploadImage(file, id);
            Assert.Equal("", result);
        }

        [Fact]
        public async Task ReplaceImage_CallsCopyTo_ReturnsImageURL()
        {
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            string imageURL = "/Assets/Uploads/1.jpg";

            var options = Options.Create(settings);

            var mockFileSystem = new Mock<IFileSystem>();
            var mockIFile = new Mock<IFile>();
            var mockFileStream = new MemoryStream();

            mockIFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockFileSystem.SetupGet(x => x.File).Returns(mockIFile.Object);

            var mockIFormFile = new Mock<IFormFile>();
            mockIFormFile.SetupGet(x => x.FileName).Returns("1.jpg");
            mockIFormFile.SetupGet(x => x.Length).Returns(15);
            mockIFormFile.Setup(x => x.CopyTo(mockFileStream))
                    .Callback<Stream>(l => l.CopyTo(mockFileStream));            

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.ReplaceImage(mockIFormFile.Object, imageURL);

            mockIFormFile.Verify(mock => mock.CopyTo(It.IsAny<Stream>()), Times.Once());
            Assert.Equal("/Assets/Uploads/1.jpg", result);
        }

        [Fact]
        public async Task ReplaceImage_ReturnsOriginalURL_ImageFileIsNull()
        {
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<FileSystem>();
            IFormFile file = null;

            string imageURL = "/Assets/Uploads/1.jpg";

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.ReplaceImage(file, imageURL);

            Assert.Equal("/Assets/Uploads/1.jpg", result);
        }

        [Fact]
        public async Task ReplaceImage_ReturnsEmptyString_ImageUrlIsNull()
        {
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<FileSystem>();
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 1, 15, "Data", "dummy.jpg");

            string imageURL = "";

            var imgService = new ImageService(options, mockFileSystem.Object);

            var result = await imgService.ReplaceImage(file, imageURL);

            Assert.Equal("", result);
        }

        [Fact]
        public void DeleteImage_CallsFileDelete_IfFileExists()
        {
            var imageURL = "/Assets/Uploads/1.jpg";
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<IFileSystem>();
            var mockIFile = new Mock<IFile>();
            mockIFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.SetupGet(x => x.File).Returns(mockIFile.Object);

            var imgService = new ImageService(options, mockFileSystem.Object);

            imgService.DeleteImage(imageURL);

            mockIFile.Verify(mock => mock.Delete(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void DeleteImage_DoesNotCallFileDelete_IfFileDoesNotExist()
        {
            var imageURL = "/Assets/Uploads/1.jpg";
            var settings = new AppSettings()
            {
                ImagesPath = "/Assets/Uploads/"
            };

            var options = Options.Create(settings);
            var mockFileSystem = new Mock<IFileSystem>();
            var mockIFile = new Mock<IFile>();
            mockIFile.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockFileSystem.SetupGet(x => x.File).Returns(mockIFile.Object);

            var imgService = new ImageService(options, mockFileSystem.Object);

            imgService.DeleteImage(imageURL);

            mockIFile.Verify(mock => mock.Delete(It.IsAny<string>()), Times.Never());
        }
    }
}