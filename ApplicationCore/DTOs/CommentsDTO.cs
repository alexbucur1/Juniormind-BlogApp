using System;
using BlogApp.Dotnet.ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Dotnet.ApplicationCore.DTOs
{
    public class CommentsDTO
    {
        public CommentsDTO() { }

        public CommentsDTO(Comment comment, string name, int repliesCount = 0)
        {
            if (comment == null)
            {
                return;
            }

            ID = comment.ID;
            UserID = comment.UserID;
            PostID = comment.PostID;
            Content = comment.Content;
            Date = comment.Date;
            ParentID = comment.ParentID;
            UserFullName = name;
            RepliesCount = repliesCount;
        }

        public int ID { get; set; }

        public string UserID { get; set; }

        public int PostID { get; set; }

        [StringLength(900, MinimumLength = 1)]
        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public int? ParentID { get; set; }

        public string UserFullName { get; set; }

        public int RepliesCount { get; set; }
    }
}
