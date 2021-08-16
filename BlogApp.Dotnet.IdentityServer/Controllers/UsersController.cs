using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Settings;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace BlogApp.Dotnet.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> GetAll(string sortOrder = "", string searchString = "", int? pageNumber = 1)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }

            var paginatedUserDTO = await _userService.GetAll(sortOrder, searchString, pageNumber);

            return Ok(paginatedUserDTO);
        }

        [HttpGet("{id}")]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> Get(string id)
        {
            if (!IsAuthorized(id))
            {
                return Forbid();
            }

            var user = await _userService.FindByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([Bind("Id, FirstName, LastName, Email, Password")] UserDTO input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
                if (await _userService.FindByEmailAsync(input.Email) != null)
                {
                    return BadRequest();
                }

                IdentityResult result = await _userService.CreateAsync(input);

                if (result.Succeeded)
                {
                    return Ok();
                }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> Edit(string id, [Bind("Id, FirstName, LastName, Email, Password")] UserDTO model)
        {
            var user = await _userService.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!IsAuthorized(id))
            {
                return Forbid();
            }

            var userByEmail = await _userService.FindByEmailAsync(model.Email);
            if (userByEmail != null && userByEmail.Id != user.Id)
            {
                return BadRequest();
            }

            ModelState.Remove("Password");

            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid.");
            }
            bool passwordChanged = false;

            if (model.Password != null)
            {
                var isValid = _userService.ValidatePassword(model.Password);

                if (isValid.Result != default)
                {
                    return BadRequest("The password is not valid.");
                }

                passwordChanged = true;
            }

            var result = await _userService.UpdateAsync(user, model, passwordChanged);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors.First().Description);
        }

        [HttpDelete("{id}")]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!IsAuthorized(id))
            {
                return Forbid();
            }

            var user = await _userService.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.Delete(id);

            return Ok();
        }

        private bool IsAuthorized(string appModelUserID)
        {
            var loggedUserID = HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Id).Value;
            return appModelUserID == loggedUserID || IsAdmin();
        }

        private bool IsAdmin()
        {
            var loggedUserRole = HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Role).Value;
            return loggedUserRole == "Administrator";
        }
    }
}