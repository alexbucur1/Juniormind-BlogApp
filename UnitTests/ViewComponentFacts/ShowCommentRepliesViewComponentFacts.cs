using System;
using System.Web;
using Xunit;
using System.Collections.Generic;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Threading.Tasks;
using System.Linq;
using BlogApp.Dotnet.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace BlogApp.Dotnet.Web.Tests
{
    public class ShowCommentRepliesViewComponentFacts
    {
        [Fact]
        public async Task ShowCommentReplies_ReturnsViewComponentResult_ModelIsTypeOfCommentViewModelEnumWithCountOfTwo()
        {
            var viewComponent = new ShowCommentRepliesViewComponent();
            var result = await viewComponent.InvokeAsync(GetTestDTOs());
            var viewResult = Assert.IsType<ViewViewComponentResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CommentsDTO>>(viewResult.ViewData.Model);
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
