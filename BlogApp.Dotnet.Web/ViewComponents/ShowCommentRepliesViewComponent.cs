using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class ShowCommentRepliesViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<CommentsDTO> comments)
        {
            return await Task.FromResult((IViewComponentResult)View("_ShowCommentReplies", comments));
        }
    }
}

