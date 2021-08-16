using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Dotnet.ApplicationCore.Models
{
    public class BlogPost
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }
        public string ImageURL { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }
}
