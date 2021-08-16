using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.ServicesTests.Helpers
{
    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager()
            : base(new FakeUserManager(),
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                default,
                //(ILogger<SignInManager<User>>)new Mock<ILogger<UserManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<User>>().Object
                )
        { }

        public bool SignOutIsCalled { get; set; }
        public bool SignInIsCalled { get; set; }

        public override Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            SignInIsCalled = true;
            return Task.FromResult(SignInResult.Success);
        }

        public override Task SignOutAsync()
        {
            SignOutIsCalled = true;
            return Task.FromResult(true);
        }
    }
}
