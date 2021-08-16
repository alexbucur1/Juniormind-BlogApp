using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel(CommentsDTO commentDTO, IEnumerable<CommentViewModel> replies = null)
        {
            Comment = commentDTO;
            Replies = replies;
            IsOwnerOrAdmin = false;
        }

        public CommentsDTO Comment { get; set; }
        public IEnumerable<CommentViewModel> Replies { get; set; }
        public bool IsOwnerOrAdmin { get; set; }
    }
}
