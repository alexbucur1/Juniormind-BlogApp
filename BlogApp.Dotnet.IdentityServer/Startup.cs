using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using BlogApp.Dotnet.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogApp.Dotnet.IdentityServer
{
    public class Startup
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _enviroment;
        public Startup(IWebHostEnvironment enviroment, IConfiguration configuration)
        {
            _enviroment = enviroment;
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            app.UseCors(
                options => options.WithOrigins(_configuration.GetSection("Origin").Value, _configuration.GetSection("NodeOrigin").Value)
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    context.Database.Migrate();
                }

                SeedData.InitializeUsers(context, userManager, roleManager).Wait();
            }

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
             
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContextPool<ApplicationContext>(options =>
                     options.UseMySql(_configuration.GetConnectionString("BlogPostContext"), ServerVersion.AutoDetect(_configuration.GetConnectionString("BlogPostContext"))));

            services.AddIdentity<User, IdentityRole>(options =>
               options.User.RequireUniqueEmail = true)
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationContext>()
               .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Endpoints.EnableAuthorizeEndpoint = true;
            })
                .AddInMemoryClients(Config.Clients)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();

            services.AddLocalApiAuthentication();

            services.AddControllers();

            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
        }
    }
}
