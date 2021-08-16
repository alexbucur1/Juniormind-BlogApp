using BlogApp.Dotnet.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace BlogApp.Dotnet.API.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactoryPosts<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationContext>));

                services.Remove(descriptor);

                services.AddDbContextPool<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTestingPosts");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactoryPosts<TStartup>>>();

                db.Database.EnsureCreated();

                try
                {
                    Utilities.InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with posts. Error: {Message}", ex.Message);
                }

                Directory.CreateDirectory(Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 6) + @"\static");
            });
        }
    }
}
