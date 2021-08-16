using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class CommentReplyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int receiverID, int postID)
        {
            var comment = new CommentsDTO();
            comment.ParentID = receiverID;
            comment.PostID = postID;
            return await Task.FromResult((IViewComponentResult)View("_CommentReply", comment));
        }
    }
}
