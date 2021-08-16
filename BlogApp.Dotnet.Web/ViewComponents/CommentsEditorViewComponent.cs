using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class CommentsEditorViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int postID)
        {
            var model = new CommentsDTO();
            model.PostID = postID;
            return await Task.FromResult(View("_CommentsEditor", model));
        }
    }
}

