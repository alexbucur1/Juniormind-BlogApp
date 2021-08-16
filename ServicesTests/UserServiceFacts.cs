using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using BlogApp.Dotnet.Services;
using BlogApp.Dotnet.Web.ServicesTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Services.Tests
{
    public class UserServiceFacts
    {
        [Fact]
        public void CreateUserReturnTrue()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);

            Assert.False(_userManager.CreateAsyncIsCalled);

            var result = _service.CreateAsync(new UserDTO
            {
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "test"
            });

            Assert.True(_userManager.CreateAsyncIsCalled);
        }

        [Fact]
        public async Task UpdateAsyncWithSamePasswordReturnTrue()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);

            Assert.False(_userManager.UpdateAsyncIsCalled);

            var result = await _service.UpdateAsync(new UserDTO
            {
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "test"
            },
            new UserDTO
            {
                FirstName = "First2",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "test"
            });

            Assert.True(_userManager.UpdateAsyncIsCalled);
            Assert.True(result.Succeeded);
        }


        [Fact]
        public async Task UpdateAsyncWithNewPasswordReturnTrue()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);

            var result = await _service.UpdateAsync(new UserDTO
            {
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "test"
            },
            new UserDTO
            {
                FirstName = "First2",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "anotherPassword"
            }, true);

            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task FindByEmailReturnUser()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);
            var result = await _service.FindByEmailAsync("first1@mail.com");

            Assert.Equal("First1", result.FirstName);
        }

        [Fact]
        public async Task FindByEmailReturnDefault()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);
            var result = await _service.FindByEmailAsync(null); ;

            Assert.True(result == default);
        }

        [Fact]
        public async Task FindByIdReturnUser()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);
            var result = await _service.FindByIdAsync("noID");

            Assert.Equal("Last1", result.LastName);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);
            Assert.False(_userManager.DeleteAsyncIsCalled);
            var result = await _service.Delete("test");

            Assert.True(result.Succeeded);
            Assert.True(_userManager.DeleteAsyncIsCalled);
        }

        [Fact]
        public async Task SignInTest()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);

            Assert.False(_signInManager.SignInIsCalled);

            var result = await _service.PasswordSignInAsync(
                new UserDTO {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "mail@mail.com",
                Password = "testpassword"}, "testString");

            Assert.True(_signInManager.SignInIsCalled);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task SignOutTest()
        {
            var _userManager = new FakeUserManager();
            var _signInManager = new FakeSignInManager();

            Assert.False(_signInManager.SignOutIsCalled);

            var settings = new AppSettings()
            {
                PageSize = 5
            };

            var appSettings = Options.Create(settings);

            var _service = new UserService(_userManager, _signInManager, GetContext(), appSettings);
            await _service.SignOutAsync();
            Assert.True(_signInManager.SignOutIsCalled);
        }

        public SignInManager<User> CreateSignInManager(UserManager<User> manager)
        {

            var context = new Mock<HttpContext>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context.Object);
            var roleManager = MockRoleManager<IdentityRole>();
            var identityOptions = new IdentityOptions();
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);
            var claimsFactory = new UserClaimsPrincipalFactory<User, IdentityRole>(manager, roleManager.Object, options.Object);
            //var logger = new TestLogger<SignInManager<PocoUser>>();
            var helper = new SignInManager<User>(manager, contextAccessor.Object, claimsFactory, options.Object, null, new Mock<IAuthenticationSchemeProvider>().Object, new DefaultUserConfirmation<User>());
            return helper;
        }

        public Mock<RoleManager<IdentityRole>> MockRoleManager<TRole>()
        {
            var store =  new Mock<IRoleStore<IdentityRole>>().Object;
            var roles = new List<IRoleValidator<IdentityRole>>();
            roles.Add(new RoleValidator<IdentityRole>());
            return new Mock<RoleManager<IdentityRole>>(store, roles, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null);
        }

        public UserManager<User> CreateUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var roleStore = store.As<IUserRoleStore<User>>();
            IList<string> roles = new List<string>() { "admin", "user" };
            roleStore.Setup(x => x.GetRolesAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(roles));
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<User>>();
            var validator = new Mock<IUserValidator<User>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<User>>();
            pwdValidators.Add(new PasswordValidator<User>());
            var userManager = new UserManager<User>(store.Object, options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<User>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }

        private static ApplicationContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(databaseName: "BlogPostDatabase1")
                            .Options;

            return new ApplicationContext(options);
        }
    }
}
