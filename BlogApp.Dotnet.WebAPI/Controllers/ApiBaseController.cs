using Microsoft.AspNetCore.Mvc;
using System.Linq;
using IdentityModel;

namespace BlogApp.Dotnet.API.Controllers
{
    public abstract class ApiBaseController<T> : Controller where T : ApiBaseController<T>
    {
        public bool IsAuthorized(string appModelUserID)
        {
            var loggedUserID = HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Id).Value;
            var loggedUserRole = HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Role).Value;
            return appModelUserID == loggedUserID || loggedUserRole.ToLower() == "administrator";
        }
    }
}