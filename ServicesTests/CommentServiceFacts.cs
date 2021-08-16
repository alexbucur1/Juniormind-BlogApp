using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Services.Tests
{
    public class CommentServiceFacts
    {

        [Fact]
        public async Task Add_AddsACommentIntoDataBase_ValidDTOAsInput()
        {
            var context = GetContext(GetValidTestComment(1));

            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            var result = await commentsService.Add(GetValidTestCommentDTO(2));
            Assert.Equal(2, result.ID);
        }

        [Fact]
        public async Task Add_DoesnotAddACommentIntoDataBase_InvalidDTOAsInput()
        {
            var comment = new Comment()
            {
                ID = 3,
                PostID = 1,
                Content = null,
                Date = DateTime.Now,
                UserID = "alex.bucur98@yahoo.com"
            };
            var context = GetContext();
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(new CommentsDTO(comment, null));
            Assert.NotEqual(comment, context.Comments.Last());
        }

        [Fact]
        public async Task Get_ReturnsCommentWithSpecifiedIDFromDB_ValidIDAsInput()
        {
            var id = 5;
            var comment = GetValidTestComment(id);
            var context = GetContext(comment);
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            var result = await commentsService.Get(id);
            Assert.IsType<CommentsDTO>(result);
            Assert.Equal(comment.ID, result.ID);
        }

        [Fact]
        public async Task Get_ReturnsEmptyDTO_InvalidIDAsInput()
        {
            var id = 100;
            var comment = GetValidTestComment(6);
            var context = GetContext(comment);
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            var result = await commentsService.Get(id);
            Assert.IsType<CommentsDTO>(result);
            Assert.Equal(0, result.ID);
        }

        [Fact]
        public async Task Remove_RemovesCommentFromDB_ValidIDAsInput()
        {
            var id = 8;
            var context = GetContext(GetValidTestComment(7));
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(GetValidTestCommentDTO(id));
            await commentsService.Remove(id);
            Assert.Equal(7, context.Comments.Last().ID);
        }

        [Fact]
        public async Task Remove_DoesNothing_InvalidIDAsInput()
        {
            var id = 100;
            var context = GetContext(GetValidTestComment(9));
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(GetValidTestCommentDTO(10));
            await commentsService.Remove(id);
            Assert.Equal(10, context.Comments.Last().ID);
        }

        [Fact]
        public async Task Remove_RemovesCommentAndChildrenFromDB_ValidIDAsInput()
        {
            var id = 12;
            var context = GetContext(GetValidTestComment(11));
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(GetValidTestCommentDTO(id));
            await commentsService.Add(GetValidTestCommentDTO(13, id));
            await commentsService.Add(GetValidTestCommentDTO(14, id));
            await commentsService.Remove(id);
            Assert.Equal(11, context.Comments.Last().ID);
        }

        [Fact]
        public async Task Update_UpdatesCommentFromDB_ValidDTOAsInput()
        {
            var newComment = new Comment()
            {
                ID = 15,
                PostID = 1,
                ParentID = 0,
                Content = "comment",
                Date = DateTime.Now,
                UserID = "alex.bucur98@yahoo.com"
            };

            var updatedComment = new CommentsDTO()
            {
                ID = 15,
                PostID = 1,
                ParentID = 0,
                Content = "updated",
                Date = DateTime.Now,
                UserFullName = "Mihai Petre",
                UserID = "alex.bucur98@yahoo.com"
            };

            var context = GetContext(newComment);
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Update(updatedComment);
            Assert.Equal("updated", context.Comments.Single(comment => comment.ID == 15).Content);
        }

        [Fact]
        public async Task GetAllByPost_ReturnsListOfCommentsDTO_ValidIdAsInput()
        {
            var id = 3;
            var context = GetContext();
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(GetValidTestCommentDTO(17, null, 3));
            await commentsService.Add(GetValidTestCommentDTO(18, null, 3));
            await commentsService.Add(GetValidTestCommentDTO(19, null, 3));
            await commentsService.Add(GetValidTestCommentDTO(20, null, 3));
            await commentsService.Add(GetValidTestCommentDTO(21, null, 3));
            var user = new User()
            {
                Id = "1234",
                FirstName = "Toma",
                LastName = "Andrei"
            };

            context.Users.Add(user);
            context.SaveChanges();
            var result =  commentsService.GetTopComments(id, 1, "");
            Assert.IsType<CommentsDTO>(result.Items.First());
            Assert.Equal(5, result.Items.Count());
        }

        [Fact]
        public async Task GetAllByPost_ReturnsEmptyList_InvalidIdAsInput()
        {
            var id = 2;
            var context = GetContext();
            var settings = new AppSettings() { PageSize = 5 };
            var appSettings = Options.Create(settings);
            var commentsService = new CommentsService(context, appSettings);
            await commentsService.Add(GetValidTestCommentDTO(22));
            await commentsService.Add(GetValidTestCommentDTO(23));
            await commentsService.Add(GetValidTestCommentDTO(24));
            await commentsService.Add(GetValidTestCommentDTO(25));
            await commentsService.Add(GetValidTestCommentDTO(26));
            var user = new User()
            {
                Id = "12345",
                FirstName = "Toma",
                LastName = "Andrei"
            };

            context.Users.Add(user);
            context.SaveChanges();
            var result = commentsService.GetTopComments(id, 1);
            Assert.Equal(0, result.Items.Count());
        }

        private static ApplicationContext GetContext(Comment comment = null)
        {
            var settings = new AppSettings();
            var appSettings = Options.Create(settings);
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "CommentsDataBase1")
                .Options;

            var context = new ApplicationContext(options);

            if (comment != null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }

            return context;
        }

        private static CommentsDTO GetValidTestCommentDTO(int id, int? parentID = null, int postID = 1)
        {
            return new CommentsDTO()
            {
                ID = id,
                PostID = postID,
                ParentID = parentID,
                Content = null,
                Date = DateTime.Now,
                UserFullName = "Mihai Petre",
                UserID = "1234"
            };
        }

        private static Comment GetValidTestComment(int id, int parentID = 0)
        {
            return new Comment()
            {
                ID = id,
                PostID = 1,
                ParentID = parentID,
                Content = "x",
                Date = DateTime.Now,
                UserID = "1234"
            };
        }
    }
}
