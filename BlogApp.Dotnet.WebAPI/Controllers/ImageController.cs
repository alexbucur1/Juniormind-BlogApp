using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Dotnet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService, IPostService postService)
        {
            _postService = postService;
            _imageService = imageService;
        }

        // PUT api/<ImageController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] ImageDTO image)
        {
            var blogPostDTO = await _postService.GetByID(image.PostID);

            if (blogPostDTO == null)
            {
                return NotFound();
            }

            if (image.File == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(blogPostDTO.ImageURL))
            {
                blogPostDTO.ImageURL = await _imageService.UploadImage(image.File, image.PostID);
            }
            else
            {
                blogPostDTO.ImageURL = await _imageService.ReplaceImage(image.File, blogPostDTO.ImageURL);
            }

            await _postService.Update(blogPostDTO);
            Log.Information("API: Image has been Updated!");

            return NoContent();
        }
    }
}
