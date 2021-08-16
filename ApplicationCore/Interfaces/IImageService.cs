using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.ApplicationCore.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImage(IFormFile imageFile, int id);
        void DeleteImage(string imageURL);
        Task<string> ReplaceImage(IFormFile imageFile, string imageURL);
    }
}
