using System.ComponentModel.DataAnnotations;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Models;

namespace BlogApp.Dotnet.Web.ViewModels
{
    public class BlogPostViewModel
    {
        public BlogPostViewModel(BlogPostDTO dto = default)
        {
            if(dto != default)
            {
                ID = dto.ID;
                Title = dto.Title;
                Content = dto.Content;
                Owner = dto.Owner;
                CreatedAt = dto.CreatedAt;
                ModifiedAt = dto.ModifiedAt;
                ImageURL = dto.ImageURL;
                UserID = dto.UserID;
                ShowModifiedDate = false;
                ShowPostImage = false;
                IsOwnerOrAdmin = false;

                if (CreatedAt.CompareTo(ModifiedAt) != 0)
                {
                    ShowModifiedDate = true;
                }

                if (!string.IsNullOrEmpty(ImageURL))
                {
                    ShowPostImage = true;
                }
            }
        }

        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Owner { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
        public string ImageURL { get; set; }
        public string UserID { get; set; }
        public bool ShowModifiedDate { get; set; }
        public bool ShowPostImage { get; set; }
        public bool IsOwnerOrAdmin { get; set; }
    }
}
