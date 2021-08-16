using AngleSharp.Html.Dom;
using BlogApp.Dotnet.DAL;
using BlogApp.Dotnet.Web.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Dotnet.Web.IntegrationTests
{
    public class PostTests : IClassFixture<CustomWebApplicationFactoryPosts<Startup>>
    {
        private readonly CustomWebApplicationFactoryPosts<Startup> _factory;
        private readonly HttpClient _client;

        public PostTests(CustomWebApplicationFactoryPosts<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Posts")]
        [InlineData("/Posts/Details/1")]
        public async Task Get_Anonymous_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Posts/Create")]
        [InlineData("/Posts/Edit/1")]
        [InlineData("/Posts/Delete/1")]
        public async Task Get_Anonymous_ReturnsRedirectToLogin(string url)
        {
            var detailsPage = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Redirect, detailsPage.StatusCode);
            Assert.Contains("Identity/Login", detailsPage.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("/Posts/Create")]
        [InlineData("/Posts/Edit/1")]
        public async Task Get_LoggedInAsAdmin_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            await LogInAsAdmin();

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Posts/Create")]
        [InlineData("/Posts/Edit/4")]
        public async Task Get_LoggedInAsUser_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            await LogInAsUser();

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Posts/Edit/1")]
        public async Task Get_LoggedInAsUser_EditReturnsForbidden(string url)
        {
            await LogInAsUser();

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Get_IndexPageHasCorrectData()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);

            var titleItems = content.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));

            Assert.Equal("TestTitle8", titleItems.First().InnerHtml);
        }

        [Theory]
        [InlineData("/Posts/Details/1")]
        public async Task Get_DetailsPageHasCorrectData(string url)
        {
            var detailsPage = await _client.GetAsync(url);
            var content = await HtmlHelpers.GetDocumentAsync(detailsPage);

            var titleItems = content.All.Where(m => m.LocalName == "h1" && m.ClassList.Contains("post-title"));
            var testPostTitle = titleItems.First();

            Assert.Equal("TestTitle1", testPostTitle.InnerHtml);
        }

        [Theory]
        [InlineData("/Posts/Edit/1")]
        public async Task Get_EditPageHasCorrectData(string url)
        {
            await LogInAsAdmin();

            var editPage = await _client.GetAsync(url);
            var content = await HtmlHelpers.GetDocumentAsync(editPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");
            var titleForm = form.GetElementsByClassName("title-form").First();

            Assert.Equal("TestTitle1", titleForm.GetAttribute("Value"));
        }

        [Fact]
        public async Task Post_Admin_Create_ReturnsRedirectToIndex()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Create");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");
            var result = await form.SubmitAsync(new
            {
                Title = "integrationtest",
                Content = "integrationtest"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var indexPage = await _client.GetAsync("/?pageNumber=1");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var testPostTitle = titleItems.First(titleItem => titleItem.InnerHtml == "integrationtest");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.NotNull(testPostTitle);
        }

        [Fact]
        public async Task Post_User_Create_ReturnsRedirectToIndex()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Create");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");
            var result = await form.SubmitAsync(new
            {
                Title = "integrationtest",
                Content = "integrationtest"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var indexPage = await _client.GetAsync("/?pageNumber=1");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var testPostTitle = titleItems.First(titleItem => titleItem.InnerHtml == "integrationtest");

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.NotNull(testPostTitle);
        }

        [Fact]
        public async Task Post_Admin_Details_DeleteReturnsRedirectToIndex()
        {
            await LogInAsAdmin();

            var detailsPage = await _client.GetAsync("/Posts/Details/4");
            var content = await HtmlHelpers.GetDocumentAsync(detailsPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='deleteButton']"),
                (IHtmlInputElement)content.QuerySelector("input[id='deleteButtonSubmit']"));

            var indexPage = await _client.GetAsync("/");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var deletedPostTitle = titleItems.FirstOrDefault(p => p.InnerHtml == "TestTitle4");

            Assert.Equal(HttpStatusCode.OK, detailsPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.Null(deletedPostTitle);
        }

        [Fact]
        public async Task Post_User_Details_DeleteReturnsRedirectToIndex()
        {
            await LogInAsUser();

            var detailsPage = await _client.GetAsync("/Posts/Details/5");
            var content = await HtmlHelpers.GetDocumentAsync(detailsPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='deleteButton']"),
                (IHtmlInputElement)content.QuerySelector("input[id='deleteButtonSubmit']"));

            var indexPage = await _client.GetAsync("/");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var deletedPostTitle = titleItems.FirstOrDefault(p => p.InnerHtml == "TestTitle5");

            Assert.Equal(HttpStatusCode.OK, detailsPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.Null(deletedPostTitle);
        }

        [Fact]
        public async Task Post_UserNotOwner_Details_DeleteReturnsForbidden()
        {
            await LogInAsUser();

            var detailsPage = await _client.GetAsync("/Posts/Details/1");
            var content = await HtmlHelpers.GetDocumentAsync(detailsPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='deleteButton']"),
                (IHtmlInputElement)content.QuerySelector("input[id='deleteButtonSubmit']"));

            Assert.Equal(HttpStatusCode.OK, detailsPage.StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Post_Admin_Edit_SaveReturnsRedirectToDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Edit/9");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");
            var result = await form.SubmitAsync(new
            {
                Title = "integrationtest",
                Content = "integrationtest"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var detailsPage = await _client.GetAsync("/Posts/Details/9");
            var detailsContent = await HtmlHelpers.GetDocumentAsync(detailsPage);
            var titleItems = detailsContent.All.Where(m => m.LocalName == "h1" && m.ClassList.Contains("post-title"));
            var testPostTitle = titleItems.First();

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/posts/Details/9", response.Headers.Location.OriginalString);
            Assert.Equal("integrationtest", testPostTitle.InnerHtml);
        }

        [Fact]
        public async Task Post_User_Edit_SaveReturnsRedirectToDetails()
        {
            await LogInAsUser();

            var createPage = await _client.GetAsync("/Posts/Edit/3");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");
            var result = await form.SubmitAsync(new
            {
                Title = "integrationtest",
                Content = "integrationtest"
            });

            var response = await _client.SendAsync(form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var detailsPage = await _client.GetAsync("/Posts/Details/3");
            var detailsContent = await HtmlHelpers.GetDocumentAsync(detailsPage);
            var titleItems = detailsContent.All.Where(m => m.LocalName == "h1" && m.ClassList.Contains("post-title"));
            var testPostTitle = titleItems.First();

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/posts/Details/3", response.Headers.Location.OriginalString);
            Assert.Equal("integrationtest", testPostTitle.InnerHtml);
        }

        [Fact]
        public async Task Post_Edit_CancelReturnsRedirectToDetails()
        {
            await LogInAsAdmin();

            var createPage = await _client.GetAsync("/Posts/Edit/2");
            var content = await HtmlHelpers.GetDocumentAsync(createPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='main-form']"),
                (IHtmlInputElement)content.QuerySelector("input[id='btn-cancel']"));

            var detailsPage = await _client.GetAsync("/Posts/Details/2");
            var detailsContent = await HtmlHelpers.GetDocumentAsync(detailsPage);
            var titleItems = detailsContent.All.Where(m => m.LocalName == "h1" && m.ClassList.Contains("post-title"));
            var originalTitle = titleItems.First();

            Assert.Equal(HttpStatusCode.OK, createPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/posts/Details/2", response.Headers.Location.OriginalString);
            Assert.Equal("TestTitle2", originalTitle.InnerHtml);
        }

        [Fact]
        public async Task Post_Admin_Edit_DeleteReturnsRedirectToIndex()
        {
            await LogInAsAdmin();

            var editPage = await _client.GetAsync("/Posts/Edit/6");
            var content = await HtmlHelpers.GetDocumentAsync(editPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='deleteButton']"),
                (IHtmlInputElement)content.QuerySelector("input[id='deleteButtonSubmit']"));

            var indexPage = await _client.GetAsync("/");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var deletedPostTitle = titleItems.FirstOrDefault(p => p.InnerHtml == "TestTitle6");

            Assert.Equal(HttpStatusCode.OK, editPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.Null(deletedPostTitle);
        }

        [Fact]
        public async Task Post_User_Edit_DeleteReturnsRedirectToIndex()
        {
            await LogInAsUser();

            var editPage = await _client.GetAsync("/Posts/Edit/7");
            var content = await HtmlHelpers.GetDocumentAsync(editPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='deleteButton']"),
                (IHtmlInputElement)content.QuerySelector("input[id='deleteButtonSubmit']"));

            var indexPage = await _client.GetAsync("/");
            var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);
            var titleItems = indexContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var deletedPostTitle = titleItems.FirstOrDefault(p => p.InnerHtml == "TestTitle7");

            Assert.Equal(HttpStatusCode.OK, editPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            Assert.Null(deletedPostTitle);
        }

        [Fact]
        public async Task Get_SearchInPosts_GetPostWithGivenValidTitle()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='search-posts']");

            var result = await form.SubmitAsync(new
            {
                SearchString = "TestTitle10"
            });

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='search-posts']"),
                (IHtmlInputElement)content.QuerySelector("input[id='search-posts-submit']"));

            var generatedPage = await _client.GetAsync("/?SearchString=TestTitle10");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var titleItems = generatedContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var count = titleItems.Count();
            var expectedPostTitle = titleItems.SingleOrDefault(p => p.InnerHtml == "TestTitle10");

            Assert.Equal(HttpStatusCode.OK, indexPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(expectedPostTitle);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task Get_SearchInPosts_GetPostWithGivenSearchStringInsideContent()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='search-posts']");

            var result = await form.SubmitAsync(new
            {
                SearchString = "TestContent17"
            });

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='search-posts']"),
                (IHtmlInputElement)content.QuerySelector("input[id='search-posts-submit']"));

            var generatedPage = await _client.GetAsync("/?SearchString=TestContent17");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var titleItems = generatedContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var count = titleItems.Count();
            var expectedPostTitle = titleItems.First(p => p.InnerHtml == "TestTitle17");

            Assert.Equal(HttpStatusCode.OK, indexPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(expectedPostTitle);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task Get_SearchInPosts_GetNoPosts_InvalidSearchString()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='search-posts']");

            var result = await form.SubmitAsync(new
            {
                SearchString = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTT"
            });

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='search-posts']"),
                (IHtmlInputElement)content.QuerySelector("input[id='search-posts-submit']"));

            var generatedPage = await _client.GetAsync("/?SearchString=TTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
            var generatedContent = await HtmlHelpers.GetDocumentAsync(generatedPage);
            var titleItems = generatedContent.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var count = titleItems.Count();

            Assert.Equal(HttpStatusCode.OK, indexPage.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Get_Pagination_GetOnlyTenPostsOutOfTotal()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);

            var titleItems = content.All.Where(m => m.LocalName == "h2" && m.ClassList.Contains("post-title"));
            var count = titleItems.Count();

            Assert.Equal(5, count);
        }

        [Fact]
        public async Task Get_Pagination_FirstPage_PreviousDisabledAndNextEnabled()
        {
            var indexPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);

            var previousButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "previous-button");
            var previousIsDisabled = previousButton.ClassList.Contains("visually-hidden");

            var nextButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "next-button");
            var nextIsEnabled = !nextButton.ClassList.Contains("visually-hidden");

            Assert.True(previousIsDisabled && nextIsEnabled);
        }

        [Fact]
        public async Task Get_Pagination_MiddlePage_PreviousEnabledAndNextEnabled()
        {
            var indexPage = await _client.GetAsync("/?pageNumber=2");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);

            var previousButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "previous-button");
            var previousIsEnabled = !previousButton.ClassList.Contains("disabled");

            var nextButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "next-button");
            var nextIsEnabled = !nextButton.ClassList.Contains("disabled");

            Assert.True(previousIsEnabled && nextIsEnabled);
        }

        [Fact]
        public async Task Get_Pagination_LastPage_PreviousEnabledAndNextDisabled()
        {
            var indexPage = await _client.GetAsync("/?pageNumber=4");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);

            var previousButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "previous-button");
            var previousIsEnabled = !previousButton.ClassList.Contains("visually-hidden");

            var nextButton = content.All.FirstOrDefault(m => m.LocalName == "a" && m.Id == "next-button");
            var nextIsDisabled = nextButton.ClassList.Contains("visually-hidden");

            Assert.True(previousIsEnabled && nextIsDisabled);
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