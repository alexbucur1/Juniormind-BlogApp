using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Services
{
    public class ImageService : IImageService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _imagePath;
        private readonly IFileSystem _fileSystem;
        private readonly string rootPath = Directory.GetCurrentDirectory();
        private readonly string currentPath = Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Last();

        public ImageService(IOptions<AppSettings> appSettings, IFileSystem fileSystem)
        {
            _appSettings = appSettings;
            _imagePath = _appSettings.Value.ImagesPath;
            _fileSystem = fileSystem;
        }
        public async Task<string> UploadImage(IFormFile imageFile, int id)
        {
            string[] validExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".tif", ".tiff" };
            string imageURL = "";
            string staticPath = rootPath.Substring(0, rootPath.Length - (currentPath.Length + 1)) + "\\static";


            if (imageFile.Length > 0)
            {
                string fileExtension = imageFile.FileName[imageFile.FileName.LastIndexOf('.')..];

                if (validExtensions.Contains(fileExtension.ToLower()))
                {
                    imageURL = _imagePath + id + fileExtension;
                        Directory.CreateDirectory(staticPath + _imagePath);
                    var filePath = Path.Combine(staticPath + _imagePath, id.ToString() + fileExtension);

                    if (!_fileSystem.File.Exists(filePath))
                    {
                        await using var stream = new FileStream(filePath, FileMode.Create);
                        imageFile.CopyTo(stream);

                        Log.Information("Image {@imageFile.FileName} uploaded.");
                    }
                }
            }

            return imageURL;
        }

        public void DeleteImage(string imageURL)
        {
            if (!string.IsNullOrEmpty(imageURL))
            {
                string fileName = imageURL[_imagePath.Length..];
                string fileExtension = fileName[fileName.LastIndexOf('.')..];
                string fileID = fileName.Substring(0, fileName.LastIndexOf('.'));
                string staticPath = rootPath.Substring(0, rootPath.Length - (currentPath.Length + 1)) + "\\static";

                var filePath = Path.Combine(staticPath + _imagePath, fileID + fileExtension);

                if (_fileSystem.File.Exists(filePath))
                {
                    _fileSystem.File.Delete(filePath);
                    Log.Information("Image {@blogPost.ID} was deleted from the server.");
                }
            }
        }

        public async Task<string> ReplaceImage(IFormFile imageFile, string imageURL)
        {
            if (imageFile != null && !string.IsNullOrEmpty(imageURL))
            {
                string fileName = imageURL[(imageURL.LastIndexOf('/') + 1)..];
                string fileID = fileName.Substring(0, fileName.LastIndexOf('.'));
                DeleteImage(imageURL);

                return await UploadImage(imageFile, Convert.ToInt32(fileID));
            }

            return imageURL;
        }
    }
}
