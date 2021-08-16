using System;
using System.Web;
using Xunit;
using Moq;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.Web.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Dotnet.Web.Tests
{
    public class CommentControllerFacts
    {
        const string Email = "alex.bucur98@yahoo.com";

        [Fact]
        public async Task Create_RedirectsToPostDetails_GivenCommentIsValid()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Add(It.IsAny<CommentsDTO>()))
           .ReturnsAsync(GetTestDTO());
            var controller = MockController(mockCommentsService, true);
            var result = await controller.Create(GetTestDTO());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
        }

        [Fact]
        public async Task Create_RedirectsToPostDetails_GivenCommentIsInvalid()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Add(It.IsAny<CommentsDTO>()))
           .ReturnsAsync(GetTestDTO());
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());
            var controller = MockController(mockCommentsService, true);
            var result = await controller.Create(GetInvalidDTO());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_RedirectsToActionResult_ValidCommentAsInput()
        {
            int id = 1;
            var mockCommentsService = new Mock<ICommentsService>();
            var controller = MockController(mockCommentsService, true);
            var result = await controller.Edit(id, GetTestDTO());
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_IdNotFound ()
        {
            int id = 2;
            var mockCommentsService = new Mock<ICommentsService>();
            var controller = MockController(mockCommentsService, false);
            var result = await controller.Edit(id, GetTestDTO());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_RedirectsToActionResult_InvalidCommentAsInput()
        {
            int id = 1;
            var mockCommentsService = new Mock<ICommentsService>();
            var controller = MockController(mockCommentsService, true);
            var result = await controller.Edit(id, GetInvalidDTO());
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Delete_RedirectsToActionResult_ValidIdAsInput()
        {
            int id = 1;
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Get(id))
            .ReturnsAsync(GetTestDTO());

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = MockController(mockCommentsService, true);
            var result = await controller.DeleteConfirmed(id);
            Assert.IsType<RedirectToActionResult>(result);
        }

        private static CommentsDTO GetTestDTO()
        {
            return new CommentsDTO()
            {
                ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "This is a test comment.",
                Date = DateTime.Now
            };
        }

        private static CommentsDTO GetInvalidDTO()
        {
            return new CommentsDTO()
            {
                ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = null,
                Date = DateTime.Now
            };
        }

        private CommentsController MockController(Mock<ICommentsService> mockService, bool hasMockHttp, string contextName = Email)
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            tempData["CommID"] = "1";
            tempData["ParentID"] = "0";

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var controller = new CommentsController(mockService.Object, mockAuthorizationService.Object)
            {
                TempData = tempData
            };

            if (hasMockHttp == true)
            {
                var mockHttpContext = new Mock<HttpContext>();
                var fakeIdentity = new GenericIdentity(contextName);
                var fakePrincipal = new GenericPrincipal(fakeIdentity, null);
                mockHttpContext.Setup(t => t.User).Returns(fakePrincipal);
                var controllerContext = new ControllerContext();
                controllerContext.HttpContext = mockHttpContext.Object;
                controller.ControllerContext = controllerContext;
            }

            return controller;
        }
    }
}
 