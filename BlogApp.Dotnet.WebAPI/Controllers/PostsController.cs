using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ApiBaseController<PostsController>
    {
        private readonly IPostService _postService;
        private readonly IImageService _imageService;

        public PostsController(IImageService imageService, IPostService postService)
        {
            _postService = postService;
            _imageService = imageService;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<PaginatedDTO<BlogPostDTO>>> GetBlogPosts(string search, int? page)
        {
            var blogPosts = GetPaginatedViewModels(await _postService.GetAll(page ??= 1, search ??= ""));
            Log.Information("API: All Paginated Blog Posts retrieved from database.");

            return blogPosts;
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostDTO>> GetBlogPost(int id)
        {
            var blogPost = await _postService.GetByID(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return blogPost;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBlogPost(int id, [Bind("ID,Title,Content,Owner,CreatedAt,UserID,ImageURL")] BlogPostDTO blogPostDTO)
        {
            if (!IsAuthorized(blogPostDTO.UserID))
            {
                return Forbid();
            }

            if (id != blogPostDTO.ID)
            {
                return NotFound();
            }

            blogPostDTO.ModifiedAt = DateTime.Now.ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            try
            {
                await _postService.Update(blogPostDTO);
            }
            catch (NullReferenceException) 
            {
                if (await _postService.GetByID(blogPostDTO.ID) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        
        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogPostDTO>> PostBlogPost([Bind("Title,Content")] BlogPostDTO blogPostDTO)
        {
            blogPostDTO.CreatedAt = DateTime.Now.ToString();
            blogPostDTO.ModifiedAt = blogPostDTO.CreatedAt;
            blogPostDTO.UserID = HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Id).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int id = await _postService.Add(blogPostDTO);
            blogPostDTO.ID = id;
            
            Log.Information($"API: BlogPost {@blogPostDTO} added to database");

            return CreatedAtAction("GetBlogPost", new { id = blogPostDTO.ID }, blogPostDTO);

        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var postDTO = await _postService.GetByID(id);

            if (postDTO == null)
            {
                return NotFound();
            }

            if (!IsAuthorized(postDTO.UserID))
            {
                return Forbid();
            }

            await _postService.Delete(id);
            _imageService.DeleteImage(postDTO.ImageURL);
            Log.Information($"API: BlogPost {@postDTO} was deleted from database");

            return NoContent();
        }

        private static PaginatedDTO<BlogPostDTO> GetPaginatedViewModels(PaginatedDTO<BlogPostDTO> paginatedDTOs)
        {
            return new PaginatedDTO<BlogPostDTO>(paginatedDTOs.Items, paginatedDTOs.PageIndex, paginatedDTOs.HasNextPage, paginatedDTOs.HasPreviousPage, paginatedDTOs.PageSize);
        }
    }
}
