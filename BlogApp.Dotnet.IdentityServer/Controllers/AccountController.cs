using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace BlogApp.Dotnet.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IOptions<AppSettings> _appSettings;

        public AccountController(IUserService userService, IConfiguration configuration, IAuthService authService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _authService = authService;
            _appSettings = appSettings;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([Bind("Email, Password, ClientInfoQuery")] ClientDTO client)
        {
            var accessTokenStartIndex = _appSettings.Value.AccesTokenPrefix;
            client.ClientInfoQuery = WebUtility.UrlDecode(client.ClientInfoQuery);
            var tokens = _authService.GetToken(client);
            var tokenInfo = tokens.Result;
            var tokenStart = tokenInfo.IndexOf(accessTokenStartIndex);
            if (tokenStart < 0)
            {
                return BadRequest();
            }

            var accessToken = tokenInfo.Substring(tokenStart + accessTokenStartIndex.Length);
            var tokenValue = accessToken.Substring(0, accessToken.IndexOf('\''));
            var user = await _userService.FindByEmailAsync(client.Email);
            var response = new ClientDTO()
            {
                Token = tokenValue,
                FullName = user.FirstName + " " + user.LastName
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var user = await _userService.FindByEmailAsync(email);

            if (user == null || password == null)
            {
                return NoContent();
            }
            var result = await _userService.PasswordSignInAsync(user, password);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            return BadRequest("The email or password are not correct.");
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return NoContent();
        }
    }
}
