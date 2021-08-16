using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class CommentEditViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CommentsDTO comment)
        {
            return await Task.FromResult((IViewComponentResult)View("_CommentEdit", comment));
        }
    }
}
