using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        public async Task<bool> IsAuthorized(string userID, IAuthorizationService _authorizationService)
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(HttpContext.User, userID, "SameOwnerPolicy");

            return result.Succeeded || HttpContext.User.IsInRole("Administrator");
        }
    }
}
