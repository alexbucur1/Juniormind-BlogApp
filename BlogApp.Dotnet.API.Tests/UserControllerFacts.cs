using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.IdentityServer.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.API.Tests
{
    public class UserControllerFacts
    {
        [Fact]
        public async Task GetUsersWhenAdminReturnsPaginatedUserDTOs()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetAll("", "", 1)).ReturnsAsync(GetTestUsers());

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.GetAll();

            var listResult = Assert.IsType<OkObjectResult>(result);
            var models = Assert.IsAssignableFrom<PaginatedDTO<UserDTO>>(listResult.Value);
            Assert.Equal(5, models.Items.Count());
            Assert.Equal("testuser@mail.com", models.Items.First().Email);
            Assert.Equal("testuser5@mail.com", models.Items.Last().Email);
        }

        [Fact]
        public async Task GetUsersWhenNotAdminReturnsForbidden()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetAll("", "", 1)).ReturnsAsync(GetTestUsers());

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("anyone", "anything")
            };

            var result = await controller.GetAll();
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetUserWhenAdminReturnsUser()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.Get("testuser1");
            var okObject = Assert.IsAssignableFrom<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserDTO>(okObject.Value);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("testuser1@mail.com", user.Email);
        }

        [Fact]
        public async Task GetUserWhenOwnerReturnsUser()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("testuser1", "User")
            };

            var result = await controller.Get("testuser1");
            var okObject = Assert.IsAssignableFrom<OkObjectResult>(result);
            var user = Assert.IsAssignableFrom<UserDTO>(okObject.Value);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("testuser1@mail.com", user.Email);
        }

        [Fact]
        public async Task GetUserWhenForbiddenReturnsForbidden()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("mother in law", "final boss")
            };

            var result = await controller.Get("testuser1");
            Assert.IsAssignableFrom<ForbidResult>(result);
        }

        [Fact]
        public async Task GetUserWhenIdIsWrongReturnsNotFound()
        {
            var mockUserService = new Mock<IUserService>();
            UserDTO nullUser = null;
            mockUserService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(nullUser);

            var controller = new UsersController(mockUserService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("admin", "Administrator")
            };

            var result = await controller.Get("someone");
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task RegisterNewUserShouldReturnOk()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO dto = null;
            mockPostService.Setup(service => service.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dto);
            mockPostService.Setup(service => service.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "trotineta28",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var result = await controller.Register(input);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RegisterNewUserWithInvalidFirstNameShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO dto = null;
            mockPostService.Setup(service => service.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dto);
            mockPostService.Setup(service => service.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(IdentityResult.Success);
            var controller = new UsersController(mockPostService.Object);
            controller.ModelState.AddModelError("FirstName", "Required");

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "trotineta28",
                FirstName = null,
                LastName = "Gura de Haur",
            };

            var result = await controller.Register(input);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RegisterNewUserWithAlreadyTakenEmailShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO dto = new UserDTO();
            mockPostService.Setup(service => service.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dto);
            mockPostService.Setup(service => service.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "trotineta28",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var result = await controller.Register(input);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RegisterNewUserWithTooSimplePasswordShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO dto = null;
            mockPostService.Setup(service => service.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dto);
            mockPostService.Setup(service => service.CreateAsync(It.IsAny<UserDTO>())).ReturnsAsync(IdentityResult.Failed());

            var controller = new UsersController(mockPostService.Object);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var result = await controller.Register(input);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditUserWhenAdminShouldReturnOk()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.Edit("testuser", input);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditUserWhenOwnerShouldReturnOk()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("testuser", "User")
            };

            var result = await controller.Edit("testuser", input);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditUserWhenForbiddenShouldReutrnForbid()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("forbid", "User")
            };

            var result = await controller.Edit("testuser", input);
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task EditUserWhenGivenIdIsNotFoundShouldReturnNotFound()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO userDTO = null;
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDTO);
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);

            var result = await controller.Edit("testuser", input);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditUserWhenInputHasNullEmailShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = null,
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };
            controller.ModelState.AddModelError("Email", "Required");

            var result = await controller.Edit("testuser", input);
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Model State is not valid.", response.Value);
        }

        [Fact]
        public async Task EditUserWhenInputHasInvalidPasswordShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(new IdentityError());
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(IdentityResult.Success);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.Edit("testuser", input);
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("The password is not valid.", response.Value);
        }

        [Fact]
        public async Task EditUserWhenUpdateFailsShouldReturnBadRequest()
        {
            var mockPostService = new Mock<IUserService>();
            var updateFailedResult = IdentityResult.Failed(new[] { new IdentityError()});
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.ValidatePassword(It.IsAny<string>())).ReturnsAsync(default(IdentityError));
            mockPostService.Setup(service => service.UpdateAsync(It.IsAny<UserDTO>(), It.IsAny<UserDTO>(), It.IsAny<bool>())).ReturnsAsync(updateFailedResult);

            var input = new UserDTO()
            {
                Email = "alex.bucur98@yahoo.com",
                Password = "troti",
                FirstName = "Alex",
                LastName = "Gura de Haur",
            };

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.Edit("testuser", input);
            var response = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Null(response.Value);
        }

        [Fact]
        public async Task DeleteUserWhenAdminShouldReturnOk()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.Delete(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };
            var result = await controller.Delete("testuser");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUserWhenOwnerShouldReturnOk()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.Delete(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("testuser", "User")
            };
            var result = await controller.Delete("testuser");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteUserWhenForbiddenShouldReturnForbidden()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.Delete(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext("forbid", "User")
            };
            var result = await controller.Delete("testuser");
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteUserWithNullGivenIdShouldReturnNotFound()
        {
            var mockPostService = new Mock<IUserService>();
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(GetValidUser());
            mockPostService.Setup(service => service.Delete(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            var result = await controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUserWithinvalidGivenIdShouldReturnNotFound()
        {
            var mockPostService = new Mock<IUserService>();
            UserDTO userDTO = null;
            mockPostService.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(userDTO);
            mockPostService.Setup(service => service.Delete(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(mockPostService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };
            var result = await controller.Delete("wrongID");
            Assert.IsType<NotFoundResult>(result);
        }

        private static PaginatedDTO<UserDTO> GetTestUsers()
        {
            var users = new List<UserDTO>()
            {
                new UserDTO()
                {
                    Id = "testuser1",
                    FirstName = "testuser",
                    LastName = "testuser",
                    Email = "testuser@mail.com",
                    Password = "testuser"
                },
                 new UserDTO()
                {
                    Id = "testuser2",
                    FirstName = "testuser2",
                    LastName = "testuser2",
                    Email = "testuser2@mail.com",
                    Password = "testuser2"
                },
                  new UserDTO()
                {
                    Id = "testuser3",
                    FirstName = "testuser3",
                    LastName = "testuser3",
                    Email = "testuser3@mail.com",
                    Password = "testuser3"
                },
                   new UserDTO()
                {
                    Id = "testuser4",
                    FirstName = "testuser4",
                    LastName = "testuser4",
                    Email = "testuser4@mail.com",
                    Password = "testuser4"
                },
                    new UserDTO()
                {
                    Id = "testuser5",
                    FirstName = "testuser5",
                    LastName = "testuser5",
                    Email = "testuser5@mail.com",
                    Password = "testuser5"
                }
            };

            return new PaginatedDTO<UserDTO>(users, 1, false, true);
        }

        private static UserDTO GetValidUser()
        {
            return new UserDTO()
            {
                Id = "testuser1",
                FirstName = "testuser1",
                LastName = "testuser1",
                Email = "testuser1@mail.com",
                Password = "testuser1"
            };
        }
    }
}
