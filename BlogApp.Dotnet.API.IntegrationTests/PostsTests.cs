using BlogApp.Dotnet.API.IntegrationTests.Helpers;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.API.IntegrationTests
{
    public class PostsTests : IClassFixture<CustomWebApplicationFactoryPosts<Startup>>
    {
        private readonly CustomWebApplicationFactoryPosts<Startup> _factory;
        private readonly HttpClient _adminClient;
        private readonly HttpClient _forbiddenClient;
        private readonly HttpClient _unauthorizedClient;
        private readonly HttpClient _ownerClient;

        public PostsTests(CustomWebApplicationFactoryPosts<Startup> factory)
        {
            _factory = factory;

            _adminClient = HttpClients.GetAuthorizedClient(factory, "Administrator", "admin");

            _forbiddenClient = HttpClients.GetAuthorizedClient(factory, "User", "forbidden");

            _ownerClient = HttpClients.GetAuthorizedClient(factory, "User", "testuser");

            _unauthorizedClient = HttpClients.GetUnauthorizedClient(factory);
        }

        [Fact]
        public async Task Get_GetBlogPosts_ReturnsAllPaginatedFirstPage()
        {
            string request = "/api/posts";
            var response = await _adminClient.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<BlogPostDTO>>();
            Assert.Equal(5, content.Items.Count());
            Assert.True(content.HasNextPage);
            Assert.False(content.HasPreviousPage);
            Assert.Equal("Integration Title Post", content.Items.ToList<BlogPostDTO>()[0].Title);
            Assert.Equal("TestTitle11", content.Items.ToList<BlogPostDTO>()[4].Title);
        }

        [Fact]
        public async Task Get_GetBlogPosts_ReturnsAllPaginatedSecondPage()
        {
            string request = "/api/posts?page=2";
            var response = await _adminClient.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<BlogPostDTO>>();
            Assert.Equal(5, content.Items.Count());
            Assert.True(content.HasNextPage);
            Assert.True(content.HasPreviousPage);
            Assert.Equal("TestTitle13", content.Items.ToList<BlogPostDTO>()[0].Title);
            Assert.Equal("TestTitle17", content.Items.ToList<BlogPostDTO>()[4].Title);
        }

        [Fact]
        public async Task Get_GetBlogPosts_SearchFilterReturnsPaginated()
        {
            string request = "/api/posts?search=TestContent5";
            var response = await _adminClient.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<PaginatedDTO<BlogPostDTO>>();
            Assert.Single(content.Items);
            Assert.False(content.HasNextPage);
            Assert.False(content.HasPreviousPage);
            Assert.Equal("TestTitle5", content.Items.ToList<BlogPostDTO>()[0].Title);
        }

        [Fact]
        public async Task Get_BlogPostDetails()
        {
            string request = "/api/posts/1";
            var response = await _adminClient.GetAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<BlogPostDTO>();
            Assert.Equal("TestTitle1", content.Title);
            Assert.Equal("/Assets/Uploads/1.jpg", content.ImageURL);
        }

        [Fact]
        public async Task Get_BlogPostDetalis_ReturnsNotFound()
        {
            string request = "/api/posts/500";
            var response = await _adminClient.GetAsync(request);

            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_PutBlogPostWhenAdmin_Success()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/1");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle1", post.Title);
            post.Title = "ModifiedTitle";

            var response = await _adminClient.PutAsJsonAsync("/api/posts/1", post);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            var updatedPost = await _adminClient.GetAsync("/api/posts/1");
            post = await updatedPost.Content.ReadAsAsync<BlogPostDTO>();
            Assert.Equal("ModifiedTitle", post.Title);
            Assert.NotEqual(post.CreatedAt, post.ModifiedAt);
        }

        [Fact]
        public async Task Put_PutBlogPostWhenOwner_Success()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/2");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle2", post.Title);
            post.Title = "ModifiedTitle";

            var response = await _ownerClient.PutAsJsonAsync("/api/posts/2", post);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var updatedPost = await _ownerClient.GetAsync("/api/posts/2");
            post = await updatedPost.Content.ReadAsAsync<BlogPostDTO>();
            Assert.Equal("ModifiedTitle", post.Title);
            Assert.NotEqual(post.CreatedAt, post.ModifiedAt);
        }

        [Fact]
        public async Task Put_PutBlogPostWhenNotOwner_Forbidden()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/2");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle2", post.Title);
            post.Title = "ModifiedTitle";
            var response = await _forbiddenClient.PutAsJsonAsync("/api/posts/2", post);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Put_PutBlogPostWhenUnauthorized_Unauthorized()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/2");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle2", post.Title);
            post.Title = "ModifiedTitle";
            var response = await _unauthorizedClient.PutAsJsonAsync("/api/posts/2", post);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Put_PutBlogPost_NotFound_EditedPostDifferentFromRequest()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/1");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle1", post.Title);
            post.Title = "ModifiedTitle";

            var response = await _adminClient.PutAsJsonAsync("/api/posts/100", post);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_PutBlogPost_NotFound_WhenPostDoesntExistsInDB()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/1");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle1", post.Title);
            post.Title = "ModifiedTitle";
            post.ID = 100;

            var response = await _adminClient.PutAsJsonAsync("/api/posts/100", post);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Put_PutBlogPost_BadRequest_ModelInvalid()
        {
            var responsePost = await _adminClient.GetAsync("/api/posts/1");
            var post = await responsePost.Content.ReadAsAsync<BlogPostDTO>();

            Assert.Equal("TestTitle1", post.Title);
            post.Title = null;

            var response = await _adminClient.PutAsJsonAsync("/api/posts/1", post);
            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_PostBlogPostWhenAdmin_ReturnsCreatedPost()
        {
            var post = new BlogPostDTO()
            {
                Title = "Integration Title Post",
                Content = "Integration Test Content"
            };

            var response = await _adminClient.PostAsJsonAsync("/api/posts", post);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

            var content = await response.Content.ReadAsAsync<BlogPostDTO>();
            Assert.Equal("Integration Title Post", content.Title);
            Assert.Equal("Integration Test Content", content.Content);
        }

        [Fact]
        public async Task Post_PostBlogPostWhenUnauthorized_Unauthorized()
        {
            var post = new BlogPostDTO()
            {
                Title = "Integration Title Post",
                Content = "Integration Test Content"
            };

            var response = await _unauthorizedClient.PostAsJsonAsync("/api/posts", post);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_PostBlogPost_BadRequestWhenModelIsInvalid()
        {
            var post = new BlogPostDTO()
            {
                Title = null,
                Content = "Integration Test Content"
            };

            var response = await _adminClient.PostAsJsonAsync("/api/posts", post);

            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteBlogPostWhenAdmin_Success()
        {
            int postID = 18;
            var response = await _adminClient.DeleteAsync($"/api/posts/{postID}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var request = await _adminClient.GetAsync($"/api/posts/{postID}");
            Assert.Equal("application/problem+json", request.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, request.StatusCode);

        }

        [Fact]
        public async Task Delete_DeleteBlogPostWhenNotOwner_Forbidden()
        {
            int postID = 17;
            var response = await _forbiddenClient.DeleteAsync($"/api/posts/{postID}");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteBlogPostWhenUnauthorized_Unauthorized()
        {
            int postID = 17;
            var response = await _unauthorizedClient.DeleteAsync($"/api/posts/{postID}");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteBlogPostWhenOwner_Success()
        {
            var post = new BlogPostDTO()
            {
                Title = "Integration Title Post",
                Content = "Integration Test Content"
            };

            var postNewPost = await _ownerClient.PostAsJsonAsync("/api/posts", post);
            var createdPost = await postNewPost.Content.ReadAsAsync<BlogPostDTO>();

            var response = await _ownerClient.DeleteAsync($"/api/posts/{createdPost.ID}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var request = await _ownerClient.GetAsync($"/api/posts/{createdPost.ID}");
            Assert.Equal("application/problem+json", request.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, request.StatusCode);
        }

        [Fact]
        public async Task Delete_DeleteBlogPost_NotFound()
        {
            int postID = 200;
            var response = await _adminClient.DeleteAsync($"/api/posts/{postID}");

            Assert.Equal("application/problem+json", response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
