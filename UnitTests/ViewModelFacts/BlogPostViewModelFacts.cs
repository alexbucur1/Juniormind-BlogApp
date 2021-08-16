using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.Web.ViewModels;
using System;
using Xunit;

namespace BlogApp.Dotnet.Web.Tests
{
    public class BlogPostViewModelFacts
    {
        [Fact]

        public void ShowModifiedDateSetsToTrue()
        {
            var modifiedPost = new BlogPost()
            {
                ID = 1,
                CreatedAt = DateTime.Parse("12-Feb-89 00:00:00"),
                ModifiedAt = DateTime.Parse("12-Feb-89 20:00:00"),
                Title = "TestPost1",
                Content = "TestContent1",
                ImageURL = "/Assets/Uploads/img1.jpg"
            };

            var modifiedPostDTO = new BlogPostDTO(modifiedPost);

            var viewModel = new BlogPostViewModel(modifiedPostDTO);

            Assert.True(viewModel.ShowModifiedDate);
        }

        [Fact]
        public void ShowModifiedDateSetsToFalse()
        {
            var unmodifiedPost = new BlogPost()
            {
                ID = 1,
                CreatedAt = DateTime.Parse("12-Feb-89 00:00:00"),
                ModifiedAt = DateTime.Parse("12-Feb-89 00:00:00"),
                Title = "TestPost1",
                Content = "TestContent1",
                ImageURL = "/Assets/Uploads/img1.jpg"
            };

            var unmodifiedPostDTO = new BlogPostDTO(unmodifiedPost);

            var viewModel = new BlogPostViewModel(unmodifiedPostDTO);

            Assert.False(viewModel.ShowModifiedDate);
        }

        [Fact]
        public void ShowPostImageSetsToTrue()
        {
            var postWithURL = new BlogPost()
            {
                ID = 1,
                CreatedAt = DateTime.Parse("12-Feb-89 00:00:00"),
                ModifiedAt = DateTime.Parse("12-Feb-89 20:00:00"),
                Title = "TestPost1",
                Content = "TestContent1",
                ImageURL = "/Assets/Uploads/img1.jpg"
            };

            var DTO = new BlogPostDTO(postWithURL);

            var viewModel = new BlogPostViewModel(DTO);

            Assert.True(viewModel.ShowPostImage);
        }

        [Fact]
        public void ShowPostImageSetsToFalse()
        {
            var postWithoutURL = new BlogPost()
            {
                ID = 1,
                CreatedAt = DateTime.Parse("12-Feb-89 00:00:00"),
                ModifiedAt = DateTime.Parse("12-Feb-89 20:00:00"),
                Title = "TestPost1",
                Content = "TestContent1",
                ImageURL = ""
            };

            var DTO = new BlogPostDTO(postWithoutURL);

            var viewModel = new BlogPostViewModel(DTO);

            Assert.False(viewModel.ShowPostImage);
        }
    }
}
