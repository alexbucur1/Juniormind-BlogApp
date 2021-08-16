using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Web.ServicesTests.Helpers
{
    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IQueryableUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object
                  )
        { }

        public bool CreateAsyncIsCalled { get; set; }
        public bool UpdateAsyncIsCalled { get; set; }
        public bool DeleteAsyncIsCalled { get; set; }

        public override IQueryable<User> Users { 
            get
            {
                IEnumerable<User> users = new List<User>()
                {
                    new User
                    {
                        FirstName = "First1",
                        LastName = "Last1",
                        Email = "first1@mail.com",
                        UserName = "first1@mail.com"
                    },
                    new User
                    {
                        FirstName = "First2",
                        LastName = "Last2",
                        Email = "first2@mail.com",
                        UserName = "first2@mail.com"
                    }
                };

                return users.AsQueryable();
            }
        }

        public override Task<IdentityResult> CreateAsync(User user, string password)
        {
            CreateAsyncIsCalled = true;
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> UpdateAsync(User user)
        {
            UpdateAsyncIsCalled = true;
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<User> FindByIdAsync(string userId)
        {

            return Task.FromResult(new User
            {
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                UserName = "first1@mail.com"
            });
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult(new User
            {
                FirstName = "First1",
                LastName = "Last1",
                Email = "first1@mail.com",
                UserName = "first1@mail.com"
            });
        }

        public override Task<IdentityResult> DeleteAsync(User user)
        {
            DeleteAsyncIsCalled = true;
            return Task.FromResult(IdentityResult.Success);
        }
    }

}
