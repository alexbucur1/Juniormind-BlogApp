using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.Web.Controllers;
using BlogApp.Dotnet.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Web.Tests
{
    public class PostControllerFacts
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfBlogPostViewModels()
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["CurrentFilter"] = "";

            var mockImageService = new Mock<IImageService>();

            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetAll(1, ""))
                .ReturnsAsync(GetTestPosts());

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object)
            {
                TempData = tempData
        };

            var result = await controller.Index("", "", 1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedDTO<BlogPostViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Items.Count());
        }

        [Fact]

        public async Task Details_ReturnsViewResult_WithBlogPostDTO()
        {
            int id = 1;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));
            var controllerContext = GetFakeControllerContext();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["Search"] = "";

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object)
            {
                TempData = tempData
            };
            controller.ControllerContext = controllerContext;

            var result = await controller.Details(id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BlogPostViewModel>(viewResult.Model);
        }

        [Fact]

        public async Task Details_ReturnsHttpNotFound_ForIdNull()
        {
            int? id = null;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["Search"] = "";

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object)
            {
                TempData = tempData
            };

            var result = await controller.Details(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task Details_ReturnsHttpNotFound_ForIdNotFound()
        {
            int id = 2;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));
            var controllerContext = GetFakeControllerContext();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["Search"] = "";

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object)
            {
                TempData = tempData
            };
            controller.ControllerContext = controllerContext;
            var result = await controller.Details(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public void GetCreate_ReturnsView()
        {
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]

        public async Task PostCreate_ReturnsRedirectToIndex()
        {
            var controllerContext = GetFakeControllerContext();

            int id = 1;
            var mockFormFile = new Mock<IFormFile>();

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(service => service.UploadImage(mockFormFile.Object, id))
                            .ReturnsAsync("urltest");

            var mockPostService = new Mock<IPostService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;

            var result = await controller.Create(GetTestPostDTO(id), mockFormFile.Object);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]

        public async Task PostCreate_ReturnsViewWithBlogPostDTO_ForModelStateInvalid()
        {
            int id = 2;
            var mockFormFile = new Mock<IFormFile>();

            var controllerContext = GetFakeControllerContext();

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(service => service.UploadImage(mockFormFile.Object, id))
                            .ReturnsAsync("urltest");

            var mockPostService = new Mock<IPostService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;
            controller.ModelState.AddModelError("Content", "Required");            

            var result = await controller.Create(GetTestPostDTO(), mockFormFile.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BlogPostDTO>(viewResult.ViewData.Model);
        }

        [Fact]

        public async Task GetEdit_ReturnsViewResult_WithBlogPostDTO()
        {
            int id = 1;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));
            var controllerContext = GetFakeControllerContext();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;

            var result = await controller.Edit(id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BlogPostViewModel>(viewResult.Model);
        }

        [Fact]

        public async Task GetEdit_ReturnsHttpNotFound_ForIdNotFound()
        {
            int id = 2;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = await controller.Edit(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task GetEdit_ReturnsHttpNotFound_ForIdNull()
        {
            int? id = null;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = await controller.Edit(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task PostEdit_ReturnsRedirectToDetails()
        {
            int id = 1;
            var mockFormFile = new Mock<IFormFile>();

            var controllerContext = GetFakeControllerContext();

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(service => service.ReplaceImage(mockFormFile.Object, "url"))
                            .ReturnsAsync("urltest");
            mockImageService.Setup(service => service.UploadImage(mockFormFile.Object, id))
                            .ReturnsAsync("urltest");

            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;

            var result = await controller.Edit(id, GetTestPostDTO(id), mockFormFile.Object);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
        }

        [Fact]

        public async Task PostEdit_ReturnsHttpNotFound_ForIdNotFound()
        {
            int id = 2;
            var mockFormFile = new Mock<IFormFile>();

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(service => service.ReplaceImage(mockFormFile.Object, "url"))
                            .ReturnsAsync("urltest");
            mockImageService.Setup(service => service.UploadImage(mockFormFile.Object, id))
                            .ReturnsAsync("urltest");

            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = await controller.Edit(id, GetTestPostDTO(), mockFormFile.Object);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task PostEdit_ReturnsHttpNotFound_ForIdNotEqualToPostID()
        {
            int id = 2;
            var mockFormFile = new Mock<IFormFile>();

            var mockImageService = new Mock<IImageService>();

            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(1));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = await controller.Edit(id, GetTestPostDTO(), mockFormFile.Object);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task PostEdit_ReturnsViewWithBlogPostDTO_WhenModelStateInvalid()
        {
            int id = 1;
            var mockFormFile = new Mock<IFormFile>();

            var controllerContext = GetFakeControllerContext();

            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(service => service.ReplaceImage(mockFormFile.Object, "url"))
                            .ReturnsAsync("urltest");
            mockImageService.Setup(service => service.UploadImage(mockFormFile.Object, id))
                            .ReturnsAsync("urltest");

            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;
            controller.ModelState.AddModelError("Content", "Required");
            var result = await controller.Edit(id, GetTestPostDTO(id), mockFormFile.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BlogPostViewModel>(viewResult.Model);
        }

        [Fact]

        public async Task Delete_ReturnsRedirectToIndex()
        {
            int id = 1;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var controllerContext = GetFakeControllerContext();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);
            controller.ControllerContext = controllerContext;

            var result = await controller.DeleteConfirmed(id);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]

        public async Task Delete_ReturnsHttpNotFound_ForIdNotFound()
        {
            int id = 2;
            var mockImageService = new Mock<IImageService>();
            var mockPostService = new Mock<IPostService>();
            mockPostService.Setup(service => service.GetByID(id))
                .ReturnsAsync(GetTestPostDTO(id));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new PostsController(mockImageService.Object, mockPostService.Object, mockAuthorizationService.Object);

            var result = await controller.DeleteConfirmed(id);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
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

        private static ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, new string[] { "User" });

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = fakeHttpContext.Object;
            controllerContext.Object.HttpContext.User = principal;

            return controllerContext.Object;
        }
    }
}
