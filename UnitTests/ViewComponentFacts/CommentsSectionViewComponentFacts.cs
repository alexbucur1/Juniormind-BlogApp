using System;
using Xunit;
using Moq;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using System.Collections.Generic;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Threading.Tasks;
using System.Linq;
using BlogApp.Dotnet.Web.ViewComponents;
using BlogApp.Dotnet.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace BlogApp.Dotnet.Web.Tests
{
    public class CommentsSectionViewComponentFacts
    {
        [Fact]
        public async Task CommentsSection_ReturnsViewComponentResult_ModelIsTypeOfCommentViewModelEnumWithCountOfTwo()
        {
            var postID = 1;

            var paginatedComms = new PaginatedDTO<CommentsDTO>(GetTestDTOs(), default, default, default);

            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.GetTopComments(postID, 1, ""))
                .Returns(paginatedComms);
            mockCommentsService.Setup(service => service.GetReplies(postID, 1, -1))
                .Returns(paginatedComms);
            mockCommentsService.Setup(service => service.GetReplies(postID, 2, -1))
                .Returns(paginatedComms);

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var viewComponent = new CommentsSectionViewComponent(mockCommentsService.Object, mockAuthorizationService.Object);
            var mockHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("alex.bucur98@yahoo.com");
            var fakePrincipal = new GenericPrincipal(fakeIdentity, null);
            mockHttpContext.Setup(t => t.User).Returns(fakePrincipal);
            var viewContext = new ViewContext();
            viewContext.HttpContext = mockHttpContext.Object;
            var viewComponentContext = new ViewComponentContext();
            viewComponentContext.ViewContext = viewContext;
            viewComponent.ViewComponentContext = viewComponentContext;
            var result = await viewComponent.InvokeAsync(postID, "");
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CommentViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        private static List<CommentsDTO> GetTestDTOs()
        {
            return new List<CommentsDTO>()
            {
                new CommentsDTO()
                {
                    ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "This is a test comment.",
                Date = DateTime.Now
                },

                new CommentsDTO()
                {
                    ID = 2,
                PostID = 1,
                UserID = "alex.bucur98@yahoo.com",
                Content = "This is a test comment.",
                Date = DateTime.Now
                }
            };
        }
    }
}
