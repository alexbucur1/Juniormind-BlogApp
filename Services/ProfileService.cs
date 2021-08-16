using BlogApp.Dotnet.ApplicationCore.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        public ProfileService(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            if (await IsAdmin(user))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "Admin"));
            }

            claims.Add(new Claim(JwtClaimTypes.Role, "User"));
            claims.Add(new Claim(JwtClaimTypes.Id, user.Id));
            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> IsAdmin(User user)
        {
            var role = await _userManager.GetRolesAsync(user);
            return role.Contains("Administrator");
        }
    }
}
