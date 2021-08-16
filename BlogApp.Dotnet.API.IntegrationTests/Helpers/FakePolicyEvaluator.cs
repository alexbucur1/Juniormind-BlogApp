using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.API.IntegrationTests.Helpers
{
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        private string _userRole;
        public FakePolicyEvaluator(string role)
        {
            _userRole = role;
        }

        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var testScheme = "Bearer";
            var claims = new List<Claim>();

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, testScheme);

            var authenticationResult = AuthenticateResult.Success(ticket);
            return await Task.FromResult(authenticationResult);
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            if (context.Request.Path.ToString().Contains("users") && _userRole != "Administrator")
            {
                return await Task.FromResult(PolicyAuthorizationResult.Forbid());
            }

            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}
