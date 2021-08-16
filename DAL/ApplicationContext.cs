using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlogApp.Dotnet.ApplicationCore.Models;

namespace BlogApp.Dotnet.DAL
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<BlogPost> BlogPosts { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Comment> Comments { get; set; }
    }
}
