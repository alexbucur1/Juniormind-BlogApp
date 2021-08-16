 using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.DAL;
using BlogApp.Dotnet.Services;
using BlogApp.Dotnet.ApplicationCore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Abstractions;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Dotnet.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                    {
                        options.ApiName = "BlogApp.Dotnet.API";
                        options.Authority = "https://localhost:5443";
                        options.RequireHttpsMetadata = false;
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "Admin");
                });

                options.AddPolicy("user", policyAdmin =>
                {
                    policyAdmin.RequireClaim("role", "User");
                });
            });

            services.AddDbContextPool<ApplicationContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("BlogPostContext"), ServerVersion.AutoDetect(Configuration.GetConnectionString("BlogPostContext"))));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IFileSystem, FileSystem>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
        {

            app.UseCors(
                options => options.WithOrigins(Configuration.GetSection("Origin").Value)
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    context.Database.Migrate();
                }

                SeedData.Initialize(context).Wait();
            }

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - (Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Last().Length + 1)), "static"))
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
