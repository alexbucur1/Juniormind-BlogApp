using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.Web.Tests
{
    public class IdentityControllerFacts
    {
        [Fact]
        public void GetRegisterReturnsView()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = controller.Register();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task PostRegisterReturnSuccessRedirectToLogin()
        {
            var user = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.CreateAsync(It.IsAny<UserDTO>())).Returns(Task.FromResult(IdentityResult.Success));
            mockUserService.Setup(service => service.FindByEmailAsync("test@mail.com")).Returns(Task.FromResult(new UserDTO()));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Register(user);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirect.ActionName);
        }

        [Fact]
        public async Task PostRegisterUserAlreadyExistsReturnView()
        {
            var user = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByEmailAsync(user.Email)).Returns(Task.FromResult(user));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Register(user);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task PostRegisterUserInsertWrongInfosReturnView()
        {
            var user = new UserDTO { Email = "testmail" };

            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();
            controller.ModelState.AddModelError("Email", "Required");

            var result = await controller.Register(user);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void GetLogInReturnView()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = controller.Login();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task PostLogInSuccesfullRedirectHome()
        {
            var user = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByEmailAsync(user.Email)).Returns(Task.FromResult(user));

            mockUserService.Setup(service => service.PasswordSignInAsync(It.IsAny<UserDTO>(), It.IsAny<string>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();
            var result = await controller.Login(user);

            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirect.Url);
        }

        [Fact]
        public async Task PostLogInSuccesfullRedirectCustom()
        {
            var user = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByEmailAsync(user.Email)).Returns(Task.FromResult(user));

            mockUserService.Setup(service => service.PasswordSignInAsync(It.IsAny<UserDTO>(), It.IsAny<string>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var iurl = new Mock<IUrlHelper>();
            iurl.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);

            IUrlHelper helper = iurl.Object;

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration)
            {
                Url = iurl.Object
            };
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Login(user, "test");

            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal("test", redirect.Url);
        }

        [Fact]
        public async Task PostLogInWrongCredintentialsReturnView()
        {
            var user = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByEmailAsync(user.Email)).Returns(Task.FromResult(user));

            mockUserService.Setup(service => service.PasswordSignInAsync(It.IsAny<UserDTO>(), It.IsAny<string>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Login(user);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task PostLogInUserDoesntExistsReturnView()
        {
            var user = new UserDTO() { Email = "inexistent@mail.com" };
            UserDTO empty = null;

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByEmailAsync(user.Email)).Returns(Task.FromResult(empty));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration); controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Login(user);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task LogOutTest()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> {{"PageSize", "5"}};
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Logout();
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteUserNotFound()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUserSucessfullRedirectToAction()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Delete("test");
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Users", redirect.ActionName);
        }

        [Fact]
        public async Task GetEditNullID()
        {
            var mockUserService = new Mock<IUserService>();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetEditUserIsNull()
        {
            UserDTO empty = null;

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(empty));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit("testString");
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetEditReturnView()
        {
            var user = GetTestUserDto();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit("testString");
            var model = Assert.IsType<ViewResult>(result);
            var modelview = Assert.IsType<UserDTO>(model.Model);
            Assert.Equal("testID", modelview.Id);
        }

        [Fact]
        public async Task PostEditUserNotFound()
        {
            UserDTO empty = null;
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(empty));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit("testString");
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostEditChangeInfosReturnSuccess()
        {
            UserDTO oldUser = GetTestUserDto();
            UserDTO newUser = GetTestUserDto();
            newUser.FirstName = "TestName";
            newUser.Password = null;

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(oldUser));
            mockUserService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), newUser, false)).Returns(Task.FromResult(IdentityResult.Success));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit("testID", newUser);
            var action = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", action.ActionName);
        }

        [Fact]
        public async Task PostEditChangeInfosAndPassword()
        {
            UserDTO oldUser = GetTestUserDto();
            UserDTO newUser = GetTestUserDto();
            newUser.FirstName = "TestName";
            newUser.Password = "TestPassword";

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(oldUser));
            mockUserService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), newUser, true)).Returns(Task.FromResult(IdentityResult.Success));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();

            var result = await controller.Edit("testID", newUser);
            var action = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", action.ActionName);
        }

        [Fact]
        public async Task PostEditUpdateFailedReturnBadRequest()
        {
            UserDTO newUser = GetTestUserDto();

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(newUser));

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());

            var inMemorySettings = new Dictionary<string, string> { { "PageSize", "5" } };
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

            var controller = new IdentityController(mockUserService.Object, mockAuthorizationService.Object, configuration);
            controller.ControllerContext = GetFakeControllerContext();
            controller.ModelState.AddModelError("Email", "Required");

            var result = await controller.Edit("testID", newUser);
            Assert.IsType<BadRequestResult>(result);
        }

        private static UserDTO GetTestUserDto()
        {
            var user = new UserDTO()
            {
                Id = "testID",
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                Password = "mypasswd"
            };

            return user;
        }

        private static List<UserDTO> GetTestUsers()
        {
            var users = new List<UserDTO>
            {
                new UserDTO(new User
                {

                    FirstName = "First1",
                    LastName = "Last1",
                    Email = "first1@mail.com",
                    UserName = "first1@mail.com"
                }),

                new UserDTO(new User
                {

                    FirstName = "First2",
                    LastName = "Last2",
                    Email = "first2@mail.com",
                    UserName = "first2@mail.com"
                })
            };

            return users;
        }

        private static ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("Administrator");
            var principal = new GenericPrincipal(fakeIdentity, new string[] { "Administrator" });

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = fakeHttpContext.Object;
            controllerContext.Object.HttpContext.User = principal;

            return controllerContext.Object;
        }
    }
}
