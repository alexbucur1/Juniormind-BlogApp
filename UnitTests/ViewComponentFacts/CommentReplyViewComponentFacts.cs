
using Xunit;
using Moq;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Threading.Tasks;
using BlogApp.Dotnet.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Dotnet.Web.Tests
{
    public class CommentReplyViewComponentFacts
    {
        [Fact]
        public async Task CommentReply_ReturnsViewComponentResult_ModelIsTypeOfCommentsDTO()
        {
            int receiverID = 6;
            int postID = 1;
            var viewComponent = new CommentReplyViewComponent();
            var result = await viewComponent.InvokeAsync(receiverID, postID);
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.IsAssignableFrom<CommentsDTO>(viewResult.ViewData.Model);
        }
    }
}
