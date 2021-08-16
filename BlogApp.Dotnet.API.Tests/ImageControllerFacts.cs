using BlogApp.Dotnet.API.Controllers;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Dotnet.API.Tests
{
    public class ImageControllerFacts
    {
        [Fact]
        public async Task PutImageReplaceExistingReturnsOK()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            var mockFile = new Mock<IFormFile>();
            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).ReturnsAsync(GetTestPostDTO());
            mockImageService.Setup(service => service.ReplaceImage(It.IsAny<IFormFile>(), It.IsAny<string>())).Verifiable();

            var controller = new ImageController(mockImageService.Object, mockPostService.Object);
            var imageInputMode = new ImageDTO
            {
                File = mockFile.Object,
                PostID = 1
            };

            var result = await controller.Put(imageInputMode);
            mockImageService.Verify(x => x.ReplaceImage(It.IsAny<IFormFile>(), It.IsAny<string>()));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutImageUploadNewReturnsOK()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            var mockFile = new Mock<IFormFile>();

            var blog = GetTestPostDTO();
            blog.ImageURL = null;

            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).ReturnsAsync(blog);
            mockImageService.Setup(service => service.UploadImage(It.IsAny<IFormFile>(), It.IsAny<int>())).Verifiable();

            var controller = new ImageController(mockImageService.Object, mockPostService.Object);
            var imageInputMode = new ImageDTO
            {
                File = mockFile.Object,
                PostID = 1
            };

            var result = await controller.Put(imageInputMode);
            mockImageService.Verify(x => x.UploadImage(It.IsAny<IFormFile>(), It.IsAny<int>()));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutImageReturnsNotFound()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            var mockFile = new Mock<IFormFile>();

            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).Returns(Task.FromResult<BlogPostDTO>(null));
            var controller = new ImageController(mockImageService.Object, mockPostService.Object);
            var imageInputMode = new ImageDTO
            {
                File = mockFile.Object,
                PostID = 1
            };

            var result = await controller.Put(imageInputMode);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutImageReturnsBadRequest()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();

            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).ReturnsAsync(GetTestPostDTO());
            var controller = new ImageController(mockImageService.Object, mockPostService.Object);
            var imageInputMode = new ImageDTO
            {
                File = null,
                PostID = 1
            };

            var result = await controller.Put(imageInputMode);
            Assert.IsType<BadRequestResult>(result);
        }

        private static BlogPostDTO GetTestPostDTO(int id = 1)
        {
            var post = new BlogPost()
            {
                ID = 1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Title = "TestPost1",
                Content = "TestContent1",
                ImageURL = "/Assets/Uploads/img1.jpg"
            };

            var dto = new BlogPostDTO(post);

            if (id == post.ID)
            {
                return dto;
            }

            return null;
        }
    }
}
