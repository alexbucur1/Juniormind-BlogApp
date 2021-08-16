using IdentityModel;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.API.Tests.Helpers
{
    public static class Helpers
    {
        public static HttpContext GetContext(string id = "admin", string role = "Administrator")
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Role, role));
            claims.Add(new Claim(JwtClaimTypes.Id, id));

            var fakeIdentity = new ClaimsIdentity(claims);
            var fakePrincipal = new ClaimsPrincipal(fakeIdentity);
            var fakeContext = new Mock<HttpContext>();
            fakeContext.Setup(t => t.User).Returns(fakePrincipal);
            fakeContext.Object.User = fakePrincipal;
            return fakeContext.Object;
        }
    }
}
