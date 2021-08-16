using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.Controllers
{
    public class IdentityController : BaseController<IdentityController>
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;

        public IdentityController(IUserService service, IAuthorizationService authorizationService, IConfiguration configuration)
        {
            _userService = service;
            _authorizationService = authorizationService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        // localhost:xxxxx/Identity/Login
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        // localhost:xxxxx/Identity/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO input)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.FindByEmailAsync(input.Email) != null)
                {
                    ModelState.AddModelError("Email", $"Email \"{input.Email}\" is already taken.");
                    return View();
                }

                IdentityResult result = await _userService.CreateAsync(input);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                //ModelState.AddModelError("Email", result.Errors.First().Description);
                ViewData["GeneralErorrs"] = result.Errors.First().Description;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserDTO input, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = await _userService.FindByEmailAsync(input.Email);

            if (user != null && input.Password != null)
            {
                var result = await _userService.PasswordSignInAsync(user, input.Password);

                if (result.Succeeded)
                {
                    if (returnUrl != null && Url.IsLocalUrl(returnUrl) && returnUrl != "/Comments/Create")
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect("/");
                }

                ModelState.AddModelError("Email", "The Email or password is incorrect");
                return View();
            }

            ViewData["GeneralErorrs"] = "Log in failed. Please try again";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Users(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "";
            ViewData["LastNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var paginatedUserDTO = await _userService.GetAll(sortOrder, searchString, pageNumber);

            return View(paginatedUserDTO);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!await IsAuthorized(id, _authorizationService))
            {
                return StatusCode(403);
            }

            await _userService.Delete(id);

            return RedirectToAction(nameof(Users));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            else if (!await IsAuthorized(user.Id, _authorizationService))
            {
                return StatusCode(403);
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, UserDTO model)
        {
            var user = await _userService.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!await IsAuthorized(user.Id, _authorizationService))
            {
                return StatusCode(403);
            }

            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                bool passwordChanged = false;

                if (model.Password != null)
                {
                    var isValid = _userService.ValidatePassword(model.Password);

                    if (isValid.Result != default)
                    {
                        ModelState.AddModelError("Password", isValid.Result.Description);
                        return View();
                    }

                    passwordChanged = true;
                }

                var result = await _userService.UpdateAsync(user, model, passwordChanged);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Posts");
                }

                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View();
            }

            return BadRequest();
        }
    }
}
