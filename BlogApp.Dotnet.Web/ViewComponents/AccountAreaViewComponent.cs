using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.DAL;
using BlogApp.Dotnet.ApplicationCore.Interfaces;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class AccountAreaViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public AccountAreaViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var model = await _userService.FindByIdAsync(name);
            return await Task.FromResult((IViewComponentResult)View("AccountArea", model));
        }
    }
}
