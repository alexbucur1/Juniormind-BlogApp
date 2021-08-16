using BlogApp.Dotnet.API.Controllers;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.API.Tests
{
    public class PostsControllerFacts
    {
        [Fact]
        public async Task GetBlogPostsReturnBlogPostDTOes()
        {
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetAll(1, "")).ReturnsAsync(GetTestPosts());

            var mockImageService = new Mock<IImageService>();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);

            var result = await controller.GetBlogPosts("", 1);

            var listResult = Assert.IsType<ActionResult<PaginatedDTO<BlogPostDTO>>>(result);
            var models = Assert.IsAssignableFrom<PaginatedDTO<BlogPostDTO>>(listResult.Value);
            Assert.Equal(2, models.Items.Count());
            Assert.Equal("TestPost1", models.Items.First().Title);
        }

        [Fact]
        public async Task GetBlogPostReturnsOneBlogPostDTO()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();

            mockPostService.Setup(service => service.GetByID(1)).ReturnsAsync(GetTestPostDTO(1));

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            var result = await controller.GetBlogPost(1);

            var jsonResult = Assert.IsType<ActionResult<BlogPostDTO>>(result);
            Assert.IsAssignableFrom<BlogPostDTO>(jsonResult.Value);
            Assert.Equal("TestPost1", jsonResult.Value.Title);
        }

        [Fact]
        public async Task GetBlogPostReturnNotFound()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(123)).ReturnsAsync(GetTestPostDTO(123));

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);

            var result = await controller.GetBlogPost(123);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBlogPostReturnsOK()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.Add(It.IsAny<BlogPostDTO>())).ReturnsAsync(It.IsAny<int>()).Verifiable();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PostBlogPost(GetTestPostDTO(1));
            mockPostService.Verify();
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostBlogPostReturnsBadRequest()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };
            controller.ModelState.AddModelError("Content", "Required");

            var result = await controller.PostBlogPost(GetTestPostDTO(1));
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task PutBlogPostReturnsOK()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.GetByID(1)).ReturnsAsync(GetTestPostDTO(1));

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PutBlogPost(1, GetTestPostDTO());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutBlogPostReturnsNotFound()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PutBlogPost(123, GetTestPostDTO());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutBlogPostReturnsBadRequest()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            controller.ModelState.AddModelError("Content", "Required");
            var result = await controller.PutBlogPost(1, GetTestPostDTO());

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutBlogPostReturnsNotFoundWhenDBisNULL()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.Update(It.IsAny<BlogPostDTO>())).ThrowsAsync(new NullReferenceException()).Verifiable();
            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).Returns(Task.FromResult<BlogPostDTO>(null)).Verifiable();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var post = GetTestPostDTO();
            post.ID = 2;
            var result = await controller.PutBlogPost(2, post);

            mockPostService.Verify();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PutBlogPostThrowsExceptionWhenDBisNULL()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.Update(It.IsAny<BlogPostDTO>())).ThrowsAsync(new NullReferenceException()).Verifiable();
            mockPostService.Setup(service => service.GetByID(It.IsAny<int>())).Returns(Task.FromResult<BlogPostDTO>(null)).Verifiable();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var post = GetTestPostDTO();
            post.ID = 2;
            var result = Assert.ThrowsAsync< DbUpdateConcurrencyException>(() => controller.PutBlogPost(2, post));
            mockPostService.Verify();
        }

        [Fact]
        public async Task DeleteBlogPostReturnsOK()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.GetByID(1)).ReturnsAsync(GetTestPostDTO(1)).Verifiable();
            mockPostService.Setup(service => service.Delete(It.IsAny<int>())).Verifiable();
            mockImageService.Setup(service => service.DeleteImage(It.IsAny<string>())).Verifiable();

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.DeleteBlogPost(1);

            mockPostService.Verify();
            mockImageService.Verify();
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBlogPostReturnNotFound()
        {
            var mockPostService = new Mock<IPostService>();
            var mockImageService = new Mock<IImageService>();
            mockPostService.Setup(service => service.GetByID(2)).ReturnsAsync(GetTestPostDTO(123));

            var controller = new PostsController(mockImageService.Object, mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.DeleteBlogPost(2);
            Assert.IsType<NotFoundResult>(result);
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

        private PaginatedDTO<BlogPostDTO> GetTestPosts()
        {
            var posts = new List<BlogPostDTO>
            {
                new BlogPostDTO(new BlogPost
                {
                    ID = 1,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "TestPost1",
                    Content = "TestContent1",
                    ImageURL = "/Assets/Uploads/img1.jpg"
                }),

                new BlogPostDTO(new BlogPost
                {
                    ID = 2,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "TestPost2",
                    Content = "TestContent2",
                    ImageURL = "/Assets/Uploads/img2.jpg"
                })
            };

            return new PaginatedDTO<BlogPostDTO>(posts, 1, false, false);
        }
    }
}
