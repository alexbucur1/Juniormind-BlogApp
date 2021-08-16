using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.Web.ViewModels;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class RepliesSectionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<CommentViewModel> replies)
        {
            return await Task.FromResult((IViewComponentResult)View("_RepliesSection", replies));
        }
    }
}
