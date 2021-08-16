using AngleSharp.Html.Dom;
using BlogApp.Dotnet.Web.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Web.IntegrationTests
{
    public class UserTests : IClassFixture<CustomWebApplicationFactoryUsers<Startup>>
    {
        private readonly CustomWebApplicationFactoryUsers<Startup> _factory;
        private readonly HttpClient _client;

        public UserTests(CustomWebApplicationFactoryUsers<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/Identity/Login")]
        [InlineData("/Identity/Register")]
        public async Task Get_Anonymous_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            await LogInAsUser(5);

            var response = await _client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Identity/Users")]
        [InlineData("/Identity/Edit/testuser2")]
        [InlineData("/Identity/Delete/7")]
        public async Task Get_LoggedInAsUser_EndpointsReturnForbidden(string url)
        {
            await LogInAsUser(5);

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("/Identity/Users")]
        [InlineData("/Identity/Edit/testuser2")]
        [InlineData("/Identity/Delete/7")]
        [InlineData("/Identity/Logout")]
        public async Task Get_Anonymous_ReturnsRedirectToLogin(string url)
        {
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("Identity/Login", response.Headers.Location.OriginalString);
        }

        [Theory]
        [InlineData("/Identity/Edit/testuser8")]
        public async Task Get_LoggedInAsOwner_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            await LogInAsUser(8);

            var response = await _client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Identity/Users")]
        [InlineData("/Identity/Edit/testuser")]
        public async Task Get_LoggedInAsAdmin_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            await LogInAsAdmin();

            var response = await _client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Post_Admin_Edit_SaveReturnsRedirectToIndex()
        {
            await LogInAsUser(7);

            var editPage = await _client.GetAsync("/Identity/Edit/testuser7");
            var content = await HtmlHelpers.GetDocumentAsync(editPage);

            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='edit-form']"),
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            Assert.Equal(HttpStatusCode.OK, editPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Get_Users_WithSearchString_PageContainsSearchedUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("/Identity/Users?SearchString=testuser6");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);
            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("first-name"));
            var count = nameItems.Count();
            var expectedUserName = nameItems.SingleOrDefault(p => p.InnerHtml.Contains("testuser6"));

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
            Assert.Equal(1, count);            
        }

        [Fact]
        public async Task Get_Users_WithFirstNameSort_PageContainsCorrectFirstUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?sortOrder=firstName_desc");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("first-name"));
            var expectedUserName = nameItems.FirstOrDefault();

            Assert.Contains("testuser9", expectedUserName.InnerHtml);
            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
        }

        [Fact]
        public async Task Get_Users_WithLastNameSort_PageContainsCorrectFirstUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?sortOrder=lastName_desc");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("last-name"));
            var expectedUserName = nameItems.FirstOrDefault();

            Assert.Contains("testuser9", expectedUserName.InnerHtml);
            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
        }

        [Fact]
        public async Task Get_Users_WithEmailSort_PageContainsCorrectFirstUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?sortOrder=email_desc");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("email"));
            var expectedUserName = nameItems.FirstOrDefault();

            Assert.Contains("testuser9@mail.com", expectedUserName.InnerHtml);
            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page1PreviousButtonIsDisabled()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var previousButton = usersContent.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("previous-page")).FirstOrDefault();

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.Contains("disabled", previousButton.ClassName);
            Assert.NotNull(previousButton);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page1NextButtonIsEnabled()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nextButton = usersContent.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("next-page")).FirstOrDefault();

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.DoesNotContain("disabled", nextButton.ClassName);
            Assert.NotNull(nextButton);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page2PreviousButtonIsEnabled()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?pageNumber=2");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var previousButton = usersContent.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("previous-page")).FirstOrDefault();

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.DoesNotContain("disabled", previousButton.ClassName);
            Assert.NotNull(previousButton);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page2NextButtonIsEnabled()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?pageNumber=2");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nextButton = usersContent.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("next-page")).FirstOrDefault();

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.DoesNotContain("disabled", nextButton.ClassName);
            Assert.NotNull(nextButton);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page3NextButtonIsDisabled()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?pageNumber=3");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nextButton = usersContent.All.Where(m => m.LocalName == "a" && m.ClassList.Contains("next-page")).FirstOrDefault();

            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.Contains("disabled", nextButton.ClassName);
            Assert.NotNull(nextButton);
        }

        [Fact]
        public async Task Get_Users_WithPagination_Page2ContainsCorrectUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?pageNumber=2");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("first-name"));
            var expectedUserName = nameItems.LastOrDefault();

            Assert.Contains("testuser7", expectedUserName.InnerHtml);
            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
        }

        [Fact]
        public async Task Get_Users_WithPaginationAndFirstNameSort_Page2ContainsCorrectUser()
        {
            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("Identity/Users?sortOrder=firstName_desc&pageNumber=2");
            var usersContent = await HtmlHelpers.GetDocumentAsync(usersPage);

            var nameItems = usersContent.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("first-name"));
            var expectedUserName = nameItems.LastOrDefault();

            Assert.Contains("testuser", expectedUserName.InnerHtml);
            Assert.Equal(HttpStatusCode.OK, usersPage.StatusCode);
            Assert.NotNull(expectedUserName);
        }

        [Fact]
        public async Task PostRegisterNewUserRedirectToLogin()
        {
            var registerPage = await _client.GetAsync("/Identity/Register");
            var content = await HtmlHelpers.GetDocumentAsync(registerPage);
            var form = (IHtmlFormElement)content.QuerySelector("form[id='main-form']");

            var result = await form.SubmitAsync(new
            {
                FirstName = "aaintegration",
                LastName = "aauser",
                email = "integuser@mail.com",
                password = "MyInteg1."
            });

            var response = await _client.SendAsync(form,
                (IHtmlButtonElement)content.All.SingleOrDefault(m => m.LocalName == "button" && m.Id == "btn-submit"));

            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("/Identity/Users");
            var userContent = await HtmlHelpers.GetDocumentAsync(usersPage);
            var users = userContent.All.Where(h => h.LocalName == "div" && h.ClassList.Contains("first-name"));
            var testFirstNameUser = users.Where(n => n.InnerHtml.Contains("aaintegration")).First();

            Assert.Equal(HttpStatusCode.OK, registerPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Identity/Login", response.Headers.Location.OriginalString);
            Assert.Contains("aaintegration", testFirstNameUser.InnerHtml);
        }

        [Fact]
        public async Task GetDeleteReturnToUsers()
        {
            await LogInAsAdmin();

            var response = await _client.GetAsync("/Identity/Delete/testuser2");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Identity/Users", response.Headers.Location.OriginalString);

            var usersPage = await _client.GetAsync("/Identity/Users");
            var content = await HtmlHelpers.GetDocumentAsync(usersPage);
            var users = content.All.Where(h => h.LocalName == "div" && h.ClassList.Contains("name"));
            var deletedUser = users.FirstOrDefault(u => u.TextContent.Contains("testuser2"));

            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task CheckIfUserCanSeeUsersButton()
        {
            await LogInAsUser(5);

            var index = await _client.GetAsync("");
            var content = await HtmlHelpers.GetDocumentAsync(index);

            var userButton = (IHtmlAnchorElement)content.All.SingleOrDefault(b => b.LocalName == "a" && b.ClassList.Contains("users"));

            Assert.Null(userButton);
        }

        [Fact]
        public async Task CheckIfAdminCanSeeUsersButton()
        {
            await LogInAsAdmin();

            var index = await _client.GetAsync("");
            var content = await HtmlHelpers.GetDocumentAsync(index);

            var userButton = (IHtmlAnchorElement)content.All.SingleOrDefault(b => b.LocalName == "a" && b.ClassList.Contains("users"));

            Assert.NotNull(userButton);
        }

        [Fact]
        public async Task CheckIfDataIsUpdatedAfterUserEdit()
        {
            await LogInAsUser(5);

            var edit = await _client.GetAsync("/Identity/Edit/testuser5");
            var content = await HtmlHelpers.GetDocumentAsync(edit);

            var form = (IHtmlFormElement)content.QuerySelector("form[id='edit-form']");
            await form.SubmitAsync(new
            {
                FirstName = "Integration",
                LastName = "testuser",
                Email = "testuser5@mail.com",
                Password = "Testuser5$"
            });


            var response = await _client.SendAsync(
                form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));


            await LogInAsAdmin();

            var usersPage = await _client.GetAsync("/Identity/Users");
            var userContent = await HtmlHelpers.GetDocumentAsync(usersPage);
            var users = userContent.All.Where(h => h.LocalName == "div" && h.ClassList.Contains("name"));
            var testFirstNameUser = users.Where(n => n.TextContent.Contains("Integration")).First();

            Assert.NotNull(testFirstNameUser);
        }

        [Fact]
        public async Task CheckIfDataIsUpdatedAfterAdminEdit()
        {
            await LogInAsAdmin();

            var edit = await _client.GetAsync("/Identity/Edit/testuser");
            var content = await HtmlHelpers.GetDocumentAsync(edit);

            var form = (IHtmlFormElement)content.QuerySelector("form[id='edit-form']");
            await form.SubmitAsync(new
            {
                FirstName = "Integration",
                LastName = "testuser",
                Email = "testuser@mail.com",
                Password = "testuser"
            });


            var response = await _client.SendAsync(
                form,
                (IHtmlInputElement)content.QuerySelector("input[id='btn-submit']"));

            var usersPage = await _client.GetAsync("/Identity/Users");
            var userContent = await HtmlHelpers.GetDocumentAsync(usersPage);
            var users = userContent.All.Where(h => h.LocalName == "div" && h.ClassList.Contains("name"));
            var testFirstNameUser = users.Where(n => n.TextContent.Contains("Integration")).First();

            Assert.NotNull(testFirstNameUser);
        }

        [Fact]
        public async Task CheckUserIsLogoutReturnLoginPage()
        {
            await LogInAsUser(5);

            var response = await _client.GetAsync("/Identity/LogOut");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Identity/Login", response.Headers.Location.OriginalString);

            var index = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(index);

            var loginButton = (IHtmlInputElement)content.All.SingleOrDefault(b => b.LocalName == "input" && b.Id == "Login");

            Assert.NotNull(loginButton);
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

        private async Task LogInAsUser(int userNumber)
        {
            var loginPage = await _client.GetAsync("/Identity/Login");
            var loginContent = await HtmlHelpers.GetDocumentAsync(loginPage);
            var loginForm = (IHtmlFormElement)loginContent.QuerySelector("form[id='login-form']");
            await loginForm.SubmitAsync(new { email = $"testuser{userNumber}@mail.com", password = $"Testuser-{userNumber}" });
            await _client.SendAsync(loginForm,
                (IHtmlButtonElement)loginContent.QuerySelector("button[id='btn-login-submit']"));
        }
    }
}
