using AngleSharp.Html.Dom;
using BlogApp.Dotnet.Web.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Web.IntegrationTests
{
    public class CommentsTests : IClassFixture<CustomWebApplicationFactoryComments<Startup>>
    {
        private readonly CustomWebApplicationFactoryComments<Startup> _factory;
        private readonly HttpClient _client;

        public CommentsTests(CustomWebApplicationFactoryComments<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_PostDetails_SeededCommentsArePresentOnDetailsPage()
        {
            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var comments = content.All.Where(m => m.ClassList.Contains("comment")).ToList();

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal("comment1", comments[0].Id);
            Assert.Equal("comment2", comments[1].Id);
        }

        [Fact]
        public async Task Comment_Create_ReturnsRedirectToLoginWhenUserNotLoggedIn()
        {
            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comments-editor']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 1,
                ParentID = 0,
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("Identity/Login", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Delete_ReturnsRedirectToLoginWhenUserNotLoggedIn()
        {
            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-delete-form-1']");
            var result = await form.SubmitAsync(new
            {
                ID = 1,
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-delete-submit-1']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("Identity/Login", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Edit_ReturnsRedirectToLoginWhenUserNotLoggedIn()
        {
            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-edit-form-1']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 1,
                ParentID = 0,
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-edit-submit-1']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("Identity/Login", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Create_UserLoggedIn_ReturnsRedirectToDetails()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comments-editor']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 20,
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Create_AdminLoggedIn_ReturnsRedirectToDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comments-editor']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 21,
                Content = "This is a new comment.",
                UserID = "admin"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Edit_UserNotOwner_Returns403()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-edit-form-11']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 1,
                ParentID = 0,
                Content = "This is a new comment.",
                UserID = "testuser2"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-edit-submit-11']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Comment_Edit_UserIsOwner_ReturnsRedirectToDetails()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-edit-form-10']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 10,
                ParentID = 0,
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-edit-submit-10']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Edit_UserIsAdmin_ReturnsRedirectToDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-edit-form-10']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 10,
                ParentID = 0,
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-edit-submit-10']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Delete_UserNotOwner_Returns403()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-delete-form-11']");
            var result = await form.SubmitAsync(new
            {
                ID = 11,
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-delete-submit-11']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Comment_Delete_UserIsOwner_ReturnsRedirectToDetails()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-delete-form-9']");
            var result = await form.SubmitAsync(new
            {
                ID = 9,
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-delete-submit-9']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Delete_UserIsAdmin_ReturnsRedirectToDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comment-delete-form-12']");
            var result = await form.SubmitAsync(new
            {
                ID = 12,
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='comment-delete-submit-12']"));

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Comment_Create_AddsNewCommentToPage_ReturnsRedirectToPostDetailsWhenLoggedIn()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='comments-editor']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 14,
                ParentID = default(int?),
                Content = "This is a new comment.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var generatedPage = await _client.GetAsync("/Posts/Details/1");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var addedComment = generatedContent.All.FirstOrDefault(m => m.LocalName == "p" && m.Id == "comment-content-14");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
            Assert.Equal("This is a new comment.", addedComment.Children.FirstOrDefault(child => child.LocalName == "span").TextContent);
        }

        [Fact]
        public async Task Comment_Create_AddsNewReplyToCommentRepliesSection()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content
                .QuerySelector("div[id='comment2']")
                .QuerySelector("div[id='replyToComment2']")
                .QuerySelector("form[id='comment-reply-form']");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 15,
                ParentID = 2,
                Content = "This is a reply.",
                UserID = "testuser"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)form
                .QuerySelector("div[class='mt-3']")
                .QuerySelector("input[id='comment-reply-submit']"));

            var generatedPage = await _client.GetAsync("/Posts/Details/1");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var addedComment = generatedContent.All.FirstOrDefault(m => m.LocalName == "p" && m.Id == "reply-content-15");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
            Assert.Equal("This is a reply.", addedComment.Children.FirstOrDefault(child => child.LocalName == "span").TextContent);
        }

        [Fact]
        public async Task Comment_Edit_UpdatesSpecifiedComment_ReturnsRedirectToPostDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.All.SingleOrDefault(m => m.LocalName == "form" && m.Id == "comment-edit-form-2");
            var result = await form.SubmitAsync(new
            {
                PostID = 1,
                ID = 2,
                ParentID = 0,
                Content = "Updated.",
                UserID = "testuser",
                Date = DateTime.Now
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.All.SingleOrDefault(m => m.LocalName == "input" && m.Id == "comment-edit-submit-2"));

            var generatedPage = await _client.GetAsync("/Posts/Details/1");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var editedComment = generatedContent.All.SingleOrDefault(m => m.Id == "comment-content-2");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Posts/Details/1", response.Headers.Location.OriginalString);
            Assert.Equal("Updated.", editedComment.Children.FirstOrDefault(child => child.LocalName == "span").TextContent);
        }

        [Fact]
        public async Task Comment_Delete_DeletesSpecifiedComment_ReturnsRedirectToPostDetails()
        {
            await LogInAsAdmin();

            var id = 9;
            var createPage = await _client.GetAsync("/Posts/Details/1?nextComms=true&prevComms=false&commsPage=1&SearchString=");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.All.SingleOrDefault(m => m.LocalName == "form" && m.Id == "comment-delete-form-8");
            var result = await form.SubmitAsync(id);

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.All.SingleOrDefault(m => m.LocalName == "input" && m.Id == "comment-delete-submit-8"));

            var generatedPage = await _client.GetAsync("/Posts/Details/1");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var comments = generatedContent.All.Where(m => m.ClassList.Contains("comment"));
            var comEightDeletedConfirmed = !comments.Any(comment => comment.Id == "comment8");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
            Assert.True(comEightDeletedConfirmed);
        }

        [Fact]
        public async Task Comment_Delete_DeletesSpecifiedCommentAndItsReplies()
        {
            await LogInAsAdmin();

            var id = 3;
            var createPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.All.SingleOrDefault(m => m.LocalName == "form" && m.Id == "comment-delete-form-3");
            var result = await form.SubmitAsync(id);

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.All.SingleOrDefault(m => m.LocalName == "input" && m.Id == "comment-delete-submit-3"));

            var generatedPage = await _client.GetAsync("/Posts/Details/1");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var comments = generatedContent.All.Where(m => m.ClassList.Contains("comment"));
            var comThreeDeletedConfirmed = !comments.Any(comment => comment.Id == "comment3");
            var comForDeletedConfirmed = !comments.Any(comment => comment.Id == "comment4");
            var comFiveDeletedConfirmed = !comments.Any(comment => comment.Id == "comment5");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Posts/Details/1", response.Headers.Location.OriginalString);
            Assert.True(comThreeDeletedConfirmed);
            Assert.True(comForDeletedConfirmed);
            Assert.True(comFiveDeletedConfirmed);
        }

        private async Task LogInAsAdmin()
        {
            var loginPage = await _client.GetAsync("/Identity/Login");
            var loginContent = await HtmlHelpers.GetDocumentAsync(loginPage);
            var loginForm = (IHtmlFormElement)loginContent.QuerySelector("form[id='login-form']");
            await loginForm.SubmitAsync(new { email = "admin@mail.com", password = "Admin12-" });
            await _client.SendAsync(loginForm,
                (IHtmlButtonElement)loginContent.QuerySelector("button[id='btn-login-submit']"));
        }

        private async Task LogInAsUser()
        {
            var loginPage = await _client.GetAsync("/Identity/Login");
            var loginContent = await HtmlHelpers.GetDocumentAsync(loginPage);
            var loginForm = (IHtmlFormElement)loginContent.QuerySelector("form[id='login-form']");
            await loginForm.SubmitAsync(new { email = "testuser@mail.com", password = "Testuser-1" });
            await _client.SendAsync(loginForm,
                (IHtmlButtonElement)loginContent.QuerySelector("button[id='btn-login-submit']"));
        }
    }
}
