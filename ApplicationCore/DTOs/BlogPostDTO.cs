using BlogApp.Dotnet.ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class BlogPostDTO
    {
        public BlogPostDTO() { }
        public BlogPostDTO(BlogPost blogPost)
        {
            ID = blogPost.ID;
            Title = blogPost.Title;
            Content = blogPost.Content;
            CreatedAt = blogPost.CreatedAt.ToString();
            ModifiedAt = blogPost.ModifiedAt.ToString();
            ImageURL = blogPost.ImageURL;
            UserID = blogPost.UserID;
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
    }
}
