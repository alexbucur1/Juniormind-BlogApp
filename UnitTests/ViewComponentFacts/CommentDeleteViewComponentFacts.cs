using System;
using System.Web;
using Xunit;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Threading.Tasks;
using BlogApp.Dotnet.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;


namespace BlogApp.Dotnet.Web.Tests
{
    public class CommentDeleteViewComponentFacts
    {
        [Fact]
        public async Task CommentDelete_ReturnsViewComponentResult_ModelIsTypeOfCommentsDTO()
        {
            var viewComponent = new CommentDeleteViewComponent();
            var result = await viewComponent.InvokeAsync(GetTestDTO());
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            Assert.IsAssignableFrom<CommentsDTO>(viewResult.ViewData.Model);
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
    }
}
