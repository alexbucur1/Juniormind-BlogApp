using Microsoft.AspNetCore.Http;

namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class ImageDTO
    {
        public IFormFile File { get; set; }
        public int PostID { get; set; }
    }
}
