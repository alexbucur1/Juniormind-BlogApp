using BlogApp.Dotnet.API.IntegrationTests.Helpers;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace BlogApp.Dotnet.API.IntegrationTests
{
    public class UsersTests : IClassFixture<CustomWebApplicationFactoryUsers<IdentityServer.Startup>>
    {
        const string AdminEmail = "admin@mail.com";
        const string AdminPassword = "Admin12-";
        const string UserEmail = "testuser@mail.com";
        const string UserPassword = "Testuser-1";

        private readonly CustomWebApplicationFactoryUsers<IdentityServer.Startup> _factory;
        private readonly HttpClient _client;
        private readonly HttpClient _authClient;

        public UsersTests(CustomWebApplicationFactoryUsers<IdentityServer.Startup> factory)
        {
            _factory = factory;

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _authClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
        }

        [Fact]
        public async Task Get_GetUsersFirstPageWhenAdmin_ReturnsPaginatedDTO()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var usersCount = content.Items.Count();
            Assert.Equal(5, usersCount); Assert.True(content.HasNextPage);
            Assert.False(content.HasPreviousPage);
            Assert.Equal("admin", content.Items.ToList()[0].Id);
        }

        [Fact]
        public async Task Get_GetUsersWhenUnauthorized_ReturnsUnauthorized()
        {
            string request = "/users";
            var response = await _client.GetAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetUsersWhenForbidden_RedirectsToAccessDenied()
        {
            _client.SetBearerToken(await GetToken(UserEmail, UserPassword));
            string request = "/users";
            var response = await _client.GetAsync(request);
            var redirectToAccessDenied = response.Headers.Location.ToString().Contains("AccessDenied");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.True(redirectToAccessDenied);
        }

        [Fact]
        public async Task Get_GetUsersSecondPage_ReturnsPaginatedDTO()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users?pageNumber=2";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var usersCount = content.Items.Count();
            Assert.Equal(5, usersCount);
            Assert.True(content.HasNextPage);
            Assert.True(content.HasPreviousPage);
            Assert.Equal("testuser5", content.Items.ToList()[3].Id);
            Assert.Equal("testuser2", content.Items.ToList()[0].Id);
        }

        [Fact]
        public async Task Get_GetUsersBySearch_ReturnsPaginatedDTO()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users?searchString=admin";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var usersCount = content.Items.Count();
            Assert.Equal(1, usersCount);
            Assert.False(content.HasNextPage);
            Assert.False(content.HasPreviousPage);
            Assert.Equal("admin", content.Items.ToList()[0].Id);
        }

        [Fact]
        public async Task Get_GetUsersSortedDescendingByEmail_ReturnsPaginatedDTO()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users?sortOrder=email_desc";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var usersCount = content.Items.Count();
            Assert.Equal(5, usersCount);
            Assert.True(content.HasNextPage);
            Assert.False(content.HasPreviousPage);
            Assert.Equal("testuser8", content.Items.ToList()[0].Id);
            Assert.Equal("testuser4", content.Items.ToList()[4].Id);
        }

        [Fact]
        public async Task Get_GetUserByIdWhenAdmin_ReturnsUser()
        {
            var expectedId = "admin";
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users/admin";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var user = await response.Content.ReadAsAsync<UserDTO>();
            Assert.Equal(user.Id, expectedId);
        }

        [Fact]
        public async Task Get_GetUserByIdWhenOwner_ReturnsUser()
        {
            var expectedId = "testuser";
            _client.SetBearerToken(await GetToken(UserEmail, UserPassword));
            string request = "/users/testuser";
            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var user = await response.Content.ReadAsAsync<UserDTO>();
            Assert.Equal(user.Id, expectedId);
        }

        [Fact]
        public async Task Get_GetUserByIdWhenIdIsWrong_ReturnsNotFound()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string request = "/users/WalkingOnTheBeachesLookingAtThePeaches";
            var response = await _client.GetAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetUserByIdWhenUnauthorized_ReturnsUnauthorized()
        {
            string request = "/users/admin";
            var response = await _client.GetAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_GetUserByIdWhenForbidden_RedirectsToAccessDenied()
        {
            _client.SetBearerToken(await GetToken(UserEmail, UserPassword));
            string request = "/users/admin";
            var response = await _client.GetAsync(request);
            var redirectToAccessDenied = response.Headers.Location.ToString().Contains("AccessDenied");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.True(redirectToAccessDenied);
        }

        [Fact]
        public async Task Post_RegisterNewUser_ReturnsOk()
        {
            string request = "/users/register";
            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "Password12-",
                FirstName = "Alex",
                LastName = "Bucur"
            };

            var response = await _client.PostAsJsonAsync(request, input);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            string getRequest = "/users?searchString=alex.bucur98";
            var getResponse = await _client.GetAsync(getRequest);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", getResponse.Content.Headers.ContentType.MediaType);

            var content = await getResponse.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var newUser = content.Items.Single();
            Assert.Equal("alex.bucur98@yahoo.com", newUser.Email);
        }

        [Fact]
        public async Task Post_RegisterNewUserWithInvalidFirstName_ReturnsBadRequest()
        {
            string request = "/users/register";
            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "Password12-",
                FirstName = "x",
                LastName = "Bucur"
            };

            var response = await _client.PostAsJsonAsync(request, input);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_RegisterNewUserWithAlreadyTakenEmail_ReturnsBadRequest()
        {
            string request = "/users/register";
            var input = new UserDTO()
            {
                Email = "testuser@mail.com",
                Password = "Password12-",
                FirstName = "Alex",
                LastName = "Bucur"
            };

            var response = await _client.PostAsJsonAsync(request, input);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_RegisterNewUserWithInvalidPassword_ReturnsBadRequest()
        {
            string request = "/users/register";
            var input = new UserDTO()
            {
                Email = "testuser123@mail.com",
                Password = "x",
                FirstName = "Alex",
                LastName = "Bucur"
            };

            var response = await _client.PostAsJsonAsync(request, input);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_EditUserProfileWhenAdmin_ReturnsOk()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var user = new UserDTO()
            {
                FirstName = "testuser11",
                LastName = "testuser11",
                Email = "testuser1111111@mail.com",
                Id = "testuser11",
                Password = "Testuser-11"
            };

            string request = "/users/testuser11";
            var response = await _client.PutAsJsonAsync(request, user);

            response.EnsureSuccessStatusCode();

            var getModifiedUser = await _client.GetAsync("/users/testuser11");
            var modifiedUser = await getModifiedUser.Content.ReadAsAsync<UserDTO>();

            Assert.Equal("testuser1111111@mail.com", modifiedUser.Email);
        }

        [Fact]
        public async Task Put_EditUserProfileWhenOwner_ReturnsOk()
        {
            _client.SetBearerToken(await GetToken(UserEmail, UserPassword));
            string getRequest = "/users/testuser";
            var getResponse = await _client.GetAsync(getRequest);

            var user = await getResponse.Content.ReadAsAsync<UserDTO>();
            user.Email = "newEmail@yahoo.com";

            string request = "/users/testuser";
            var response = await _client.PutAsJsonAsync(request, user);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", getResponse.Content.Headers.ContentType.MediaType);

            var getModifiedUser = await _client.GetAsync("/users/testuser");
            var modifiedUser = await getModifiedUser.Content.ReadAsAsync<UserDTO>();

            Assert.Equal("newEmail@yahoo.com", modifiedUser.Email);
        }

        [Fact]
        public async Task Put_EditUserProfileWhenUnauthorized_ReturnsUnauthorized()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var user = new UserDTO()
            {
                FirstName = "testuser2",
                LastName = "testuser2",
                Email = "testuser22@mail.com",
                Password = "",
                Id = ""
            };

            string request = "/users/testuser2";
            _client.SetBearerToken("my client is heart broken, it deserves a false token");
            var response = await _client.PutAsJsonAsync(request, user);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Put_EditUserProfileWhenForbidden_RedirectsToAccessDenied()
        {
            var user = new UserDTO()
            {
                FirstName = "testuser2",
                LastName = "testuser2",
                Email = "testuser22@mail.com",
                Password = "",
                Id = ""
            };

            string request = "/users/testuser2";
            _client.SetBearerToken(await GetToken(UserEmail, UserPassword));
            var response = await _client.PutAsJsonAsync(request, user);
            var redirectToAccessDenied = response.Headers.Location.ToString().Contains("AccessDenied");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.True(redirectToAccessDenied);
        }

        [Fact]
        public async Task Put_EditUserWithInvalidIdReturnsNotFound()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var user = new UserDTO()
            {
                FirstName = "testuser2",
                LastName = "testuser2",
                Email = "testuser22@mail.com",
                Password = "Testuser-2",
                Id = "2"
            };

            string request = "/users/wrongId";
            var response = await _client.PutAsJsonAsync(request, user);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_EditUserAndGiveInvalidFirstName_ReturnsBadRequest()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var user = new UserDTO()
            {
                FirstName = "X",
                LastName = "testuser2",
                Email = "testuser22@mail.com",
                Password = "Testuser-2",
                Id = "2"
            };

            string request = "/users/testuser2";
            var response = await _client.PutAsJsonAsync(request, user);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Put_EditUserAndGiveInvalidPassword_ReturnsBadRequest()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var user = new UserDTO()
            {
                FirstName = "testuser2",
                LastName = "X",
                Email = "testuser2@mail.com",
                Password = "Testuser-2",
                Id = "2"
            };

            string request = "/users/testuser2";
            var response = await _client.PutAsJsonAsync(request, user);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteUserWithSpecifiedIdWhenAdmin_ReturnsOk()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var request = "/users/testuser3";
            var response = await _client.DeleteAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var getRequest = "/users";
            var getFirstPage = await _client.GetAsync(getRequest);
            var firstPage = await getFirstPage.Content.ReadAsAsync<PaginatedDTO<UserDTO>>();
            var userIsDeleted = firstPage.Items.FirstOrDefault(user => user.FirstName == "testuser3") == null;
            Assert.True(userIsDeleted);
        }

        [Fact]
        public async Task Delete_DeleteUserWithSpecifiedIdWhenOwner_ReturnsOk()
        {
            _client.SetBearerToken(await GetToken("testuser9@mail.com", "Testuser-9"));
            var request = "/users/testuser9";
            var response = await _client.DeleteAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var getRequest = "/users/testuser9";
            var getFirstPage = await _client.GetAsync(getRequest);
            Assert.Equal(HttpStatusCode.NotFound, getFirstPage.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteUserWhenUnauthorized_ReturnsUnauthorized()
        {
            var request = "/users/testuser3";
            var response = await _client.DeleteAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteUserWhenForbidden_RedirectsToAccessDenied()
        {
            _client.SetBearerToken(await GetToken("testuser12@mail.com", "Testuser-12"));
            var request = "/users/testuser10";
            var response = await _client.DeleteAsync(request);
            var redirectToAccessDenied = response.Headers.Location.ToString().Contains("AccessDenied");
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.True(redirectToAccessDenied);
        }

        [Fact]
        public async Task Delete_DeleteUserWhenIdIsInvalid_ReturnsNotFound()
        {
            _client.SetBearerToken(await GetToken(AdminEmail, AdminPassword));
            var request = "/users/invalidID";
            var response = await _client.DeleteAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private async Task<string> GetToken(string email, string password)
        {
            var accessTokenStart = "'access_token' value='";
            var discoveryDocument = await _authClient.GetDiscoveryDocumentAsync();
            UriBuilder builder = new UriBuilder(discoveryDocument.AuthorizeEndpoint);
            builder.Query = "client_id=Angular.client&redirect_uri=https://localhost:5002/api/Posts&response_type=token id_token&scope=openid user_profile Api.Scope IdentityServerApi&state=123456&nonce=123456&response_mode=form_post";
            var result = _authClient.GetAsync(builder.Uri).Result;
            var uri = result.RequestMessage.RequestUri.ToString() + $"&email={email}&password={password}";
            var tokens = await _authClient.PostAsync(uri, new StringContent(""));
            var tokensContent = await tokens.Content.ReadAsStringAsync();
            var trimedContent = tokensContent.Substring(tokensContent.IndexOf(accessTokenStart) + accessTokenStart.Length);
            var accessToken = trimedContent.Substring(0, trimedContent.IndexOf('\''));
            return accessToken;
        }
    }
}