using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Services.Tests
{
    public class PostServiceFacts
    {
        [Fact]
        public async Task GetAll_ReturnsAllDTOsOrderedByDateCreated_ContentIsTruncated()
        {
            var posts = GetFakePosts().AsQueryable();
            var settings = new AppSettings() { ContentPreviewTruncationTreshold = 5, PageSize = 5 };
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                .Options;

            using var context = new ApplicationContext(options);

            context.BlogPosts.AddRange(posts);
            var user = new User()
            {
                Id = "testuser",
                Email = "testuser3@mail.com",
                UserName = "testuser3@mail.com",
                FirstName = "testuser",
                LastName = "testuser",
            };
            context.Users.Add(user);
            context.SaveChanges();

            var postService = new PostService(context, appSettings);

            var result = await postService.GetAll(1, "");

            Assert.Equal(8, result.Items.ElementAt(0).Content.Length);
            Assert.IsType<BlogPostDTO>(result.Items.ElementAt(0));
        }

        [Fact]
        public async Task GetByID_ReturnsDTO()
        {
            int id = 6;
            var post = new BlogPost
            {
                ID = 6,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Title = "TestPost3",
                Content = "TestContent3",
                ImageURL = "/Assets/Uploads/img3.jpg",
                UserID = "testuser2"
            };
            var settings = new AppSettings();
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                .Options;

            using var context = new ApplicationContext(options);
            context.BlogPosts.Add(post);
            var user = new User()
            {
                Id = "testuser2",
                Email = "testuser2@mail.com",
                UserName = "testuser2@mail.com",
                FirstName = "testuser2",
                LastName = "testuser2",
            };
            context.Users.Add(user);
            context.SaveChanges();

            var postService = new PostService(context, appSettings);

            var result = await postService.GetByID(id);

            Assert.IsType<BlogPostDTO>(result);
            Assert.Equal("TestPost3", result.Title);
        }

        [Fact]
        public async Task Add_AddsPostToDB_ReturnsID()
        {
            var newPost = new BlogPost
            {
                ID = 4,
                Title = "addtest",
                Content = "addtest",                
            };
            var newPostDTO = new BlogPostDTO(newPost);
            newPostDTO.UserID = "testID";

            var settings = new AppSettings();
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                .Options;

            using var context = new ApplicationContext(options);
            var user = new User() {
                Id = "testID",
                Email = "testEmail",
                FirstName = "first",
                LastName = "last"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var postService = new PostService(context, appSettings);

            var result = await postService.Add(newPostDTO);
            var post = await context.BlogPosts.FirstOrDefaultAsync(post => post.ID == 4);

            Assert.Equal(4, result);
            Assert.Equal("addtest", post.Title);
        }

        [Fact]
        public async Task Update_UpdatesDBPostEntry()
        {
            var newPost = new BlogPost
            {
                ID = 5,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Title = "TestPost5",
                Content = "TestContent5",
                ImageURL = "/Assets/Uploads/img5.jpg",
                UserID = "testuser"
            };

            var updatedPost = new BlogPost
            {
                ID = 5,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Title = "updatetest",
                Content = "TestContent5",
                ImageURL = "/Assets/Uploads/img5.jpg",
                UserID = "testuser"
            };
            var updatedPostDTO = new BlogPostDTO(updatedPost);

            var settings = new AppSettings();
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                .Options;

            using var context = new ApplicationContext(options);
            var user = new User()
            {
                Id = "testuser3",
                Email = "testuser3@mail.com",
                UserName = "testuser3@mail.com",
                FirstName = "testuser3",
                LastName = "testuser3",
            };
            context.Users.Add(user);            
            context.BlogPosts.Add(newPost);
            await context.SaveChangesAsync();

            var postService = new PostService(context, appSettings);

            await postService.Update(updatedPostDTO);

            var updated = await postService.GetByID(5);
            Assert.Equal("updatetest", updated.Title);
        }

        [Fact]
        public async Task Delete_DeletesPostFromDB()
        {
            var newPost = new BlogPost
            {
                ID = 6,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Title = "TestPost6",
                Content = "TestContent6",
                ImageURL = "/Assets/Uploads/img6.jpg"
            };

            int id = 6;
            var settings = new AppSettings();
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                .Options;

            using var context = new ApplicationContext(options);
            context.BlogPosts.Add(newPost);
            context.SaveChanges();

            var postService = new PostService(context, appSettings);

            await postService.Delete(id);

            var deletedPost = await postService.GetByID(id);

            Assert.Null(deletedPost);
        }

        private static List<BlogPost> GetFakePosts()
        {
            var posts = new List<BlogPost>
            {
                new BlogPost
                {
                    ID = 1,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "TestPost1",
                    Content = "TestContent1",
                    ImageURL = "/Assets/Uploads/img1.jpg",
                    UserID = "testuser"                    
                },

                new BlogPost
                {
                    ID = 2,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "TestPost2",
                    Content ="TestContent2",
                    ImageURL = "/Assets/Uploads/img2.jpg",
                    UserID = "testuser"
                }
            };

            return posts;
        }
    }
}
