using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.Controllers
{
    public class PostsController : BaseController<PostsController>
    {
        private readonly IPostService _postService;
        private readonly IImageService _imageService;
        private readonly IAuthorizationService _authorizationService;

        public PostsController(IImageService imageService, IPostService postService, IAuthorizationService authorizationService)
        {
            _postService = postService;
            _imageService = imageService;
            _authorizationService = authorizationService;
        }

        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, string currentFilter, int? pageNumber)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            TempData["CurrentFilter"] = searchString;

            var blogPostViewModels = GetPaginatedViewModels(await _postService.GetAll(pageNumber ??= 1, searchString ??= ""));
            TempData["NoResultsFound"] = blogPostViewModels.Items.Count() == 0 ? "true" : "false";

            Log.Information("All BlogPosts retrieved from database.");

            return View(blogPostViewModels);
        }


        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id,  string searchString ="", int commsPage = 1, bool nextComms = false, bool prevComms = false)
        {
            TempData["Search"] = searchString;
            TempData.Keep("Search");

            if (nextComms)
            {
                TempData["CurrentCommsPage"] = commsPage + 1;
            }
            if (prevComms)
            {
                TempData["CurrentCommsPage"] = commsPage - 1;
            }
            if (!nextComms && !prevComms)
            {
                TempData["CurrentCommsPage"] = commsPage;
            }

            if (id == null)
            {
                return NotFound();
            }

            var postDTO = await _postService.GetByID((int)id);

            if (postDTO == null)
            {
                Log.Information("BlogPost {@id} not found.", id);
                return NotFound();
            }

            var viewModel = new BlogPostViewModel(postDTO) { IsOwnerOrAdmin = await IsAuthorized(postDTO.UserID, _authorizationService) };

            Log.Information("BlogPost {@blogPost} retrieved from database", postDTO);

            return View(viewModel);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Content,Owner,CreatedAt,ModifiedAt,ImageURL")] BlogPostDTO blogPostDTO, IFormFile imageFile)
        {
            blogPostDTO.CreatedAt = DateTime.Now.ToString();
            blogPostDTO.ModifiedAt = blogPostDTO.CreatedAt;
            blogPostDTO.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                return View(blogPostDTO);
            }

            int id = await _postService.Add(blogPostDTO);
            blogPostDTO.ID = id;

            if (imageFile != null)
            {
                blogPostDTO.ImageURL = await _imageService.UploadImage(imageFile, id);
                await _postService.Update(blogPostDTO);
            }

            Log.Information("BlogPost {@blogPostDTO} added to database", blogPostDTO);

            return RedirectToAction(nameof(Index));
        }

        // GET: BlogPosts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postDTO = await _postService.GetByID((int)id);

            if (postDTO == null)
            {
                return NotFound();
            }

            if (!await IsAuthorized(postDTO.UserID, _authorizationService))
            {
                return StatusCode(403);
            }

            return View(new BlogPostViewModel(postDTO));
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Content,Owner,CreatedAt,UserID,ImageURL")] BlogPostDTO blogPostDTO, IFormFile imageFile)
        {
            if (id != blogPostDTO.ID)
            {
                return NotFound();
            }

            if (!await IsAuthorized(blogPostDTO.UserID, _authorizationService))
            {
                return StatusCode(403);
            }

            blogPostDTO.ModifiedAt = DateTime.Now.ToString();

            if (!ModelState.IsValid)
            {
                return View(new BlogPostViewModel(blogPostDTO));
            }

            try
            {
                if (imageFile != null)
                {
                    if (string.IsNullOrEmpty(blogPostDTO.ImageURL))
                    {
                        blogPostDTO.ImageURL = await _imageService.UploadImage(imageFile, id);
                    }
                    else
                    {
                        blogPostDTO.ImageURL = await _imageService.ReplaceImage(imageFile, blogPostDTO.ImageURL);
                    }
                }

                await _postService.Update(blogPostDTO);
            }
            catch (DbUpdateConcurrencyException) when (_postService.GetByID(blogPostDTO.ID) == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id });
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postDTO = await _postService.GetByID(id);

            if (postDTO == null)
            {
                return NotFound();
            }

            if (!await IsAuthorized(postDTO.UserID, _authorizationService))
            {
                return StatusCode(403);
            }

            _imageService.DeleteImage(postDTO.ImageURL);

            await _postService.Delete(id);

            Log.Information("BlogPost {@blogPost} was deleted from database", postDTO);

            return RedirectToAction(nameof(Index));
        }

        private static PaginatedDTO<BlogPostViewModel> GetPaginatedViewModels(PaginatedDTO<BlogPostDTO> paginatedDTOs)
        {
            var paginatedViewModels = paginatedDTOs.Items.Select(dto => new BlogPostViewModel(dto));
            return new PaginatedDTO<BlogPostViewModel>(paginatedViewModels, paginatedDTOs.PageIndex, paginatedDTOs.HasNextPage, paginatedDTOs.HasPreviousPage, paginatedDTOs.PageSize);
        }

    }
}

