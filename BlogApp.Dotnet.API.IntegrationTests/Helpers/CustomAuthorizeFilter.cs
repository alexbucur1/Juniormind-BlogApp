using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlogApp.Dotnet.API.IntegrationTests.Helpers
{
    class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _userRole;
        private readonly string _userID;
        public CustomAuthorizeFilter(string userRole, string userID)
        {
            _userID = userID;
            _userRole = userRole;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var claims = new[]
           {
             new Claim(JwtClaimTypes.Role, _userRole),
            new Claim(JwtClaimTypes.Id, _userID)
        };

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);
            context.HttpContext.User = principal;
        }
    }
}
