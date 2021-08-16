using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.Authorization
{
    public class UserIsOwnerAuthorizationHandler : AuthorizationHandler<SameOwnerRequirement, string>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SameOwnerRequirement requirement,
                                                       string resource)
        {
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) == resource)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
