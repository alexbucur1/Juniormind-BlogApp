
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
    public class CommentsEditorViewComponentFacts
    {
        [Fact]
        public async Task CommentsEditor_ReturnsViewComponentResult_ModelIsTypeOfCommentsDTO()
        {
            int postID = 1;
            var viewComponent = new CommentsEditorViewComponent();
            var result = await viewComponent.InvokeAsync(postID);
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.IsAssignableFrom<CommentsDTO>(viewResult.ViewData.Model);
        }
    }
}
