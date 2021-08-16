using BlogApp.Dotnet.ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.DAL
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {                        
            await SeedUsers(context, userManager, roleManager);
            context.SaveChanges();
            SeedBlogPosts(context);
            context.SaveChanges();
        }

        public static async Task Initialize(ApplicationContext context)
        {
            SeedBlogPosts(context);
            context.SaveChanges();
        }

        public static async Task InitializeUsers(ApplicationContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedUsers(context, userManager, roleManager);
            context.SaveChanges();
        }

        private static async Task SeedUsers(ApplicationContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (context.Users.Any())
            {
                return;
            }

            var admin = new User
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@mail.com",
                UserName = "admin",
                Id = "admin"
            };

            var testuser = new User
            {
                FirstName = "testuser",
                LastName = "testuser",
                Email = "testuser@mail.com",
                UserName = "testuser",
                Id = "testuser"
            };

            var testuser2 = new User
            {
                FirstName = "testuser2",
                LastName = "testuser2",
                Email = "testuser2@mail.com",
                UserName = "testuser2",
                Id = "testuser2"
            };

            var testuser3 = new User
            {
                FirstName = "testuser3",
                LastName = "testuser3",
                Email = "testuser3@mail.com",
                UserName = "testuser3",
                Id = "testuser3"
            };

            var testuser4 = new User
            {
                FirstName = "testuser4",
                LastName = "testuser4",
                Email = "testuser4@mail.com",
                UserName = "testuser4",
                Id = "testuser4"
            };

            var testuser5 = new User
            {
                FirstName = "testuser5",
                LastName = "testuser5",
                Email = "testuser5@mail.com",
                UserName = "testuser5",
                Id = "testuser5"
            };

            var testuser6 = new User
            {
                FirstName = "testuser6",
                LastName = "testuser6",
                Email = "testuser6@mail.com",
                UserName = "testuser6",
                Id = "testuser6"
            };
             var testuser7 = new User
             {
                 FirstName = "testuser7",
                 LastName = "testuser7",
                 Email = "testuser7@mail.com",
                 UserName = "testuser7",
                 Id = "testuser7"
             };

            var testuser8 = new User
            {
                FirstName = "testuser8",
                LastName = "testuser8",
                Email = "testuser8@mail.com",
                UserName = "testuser8",
                Id = "testuser8"
            };

            var testuser9 = new User
            {
                FirstName = "testuser9",
                LastName = "testuser9",
                Email = "testuser9@mail.com",
                UserName = "testuser9",
                Id = "testuser9"
            };

            var testuser10 = new User
            {
                FirstName = "testuser10",
                LastName = "testuser10",
                Email = "testuser10@mail.com",
                UserName = "testuser10",
                Id = "testuser10"
            };

            var testuser11 = new User
            {
                FirstName = "testuser11",
                LastName = "testuser11",
                Email = "testuser11@mail.com",
                UserName = "testuser11",
                Id = "testuser11"
            };

            var testuser12 = new User
            {
                FirstName = "testuser12",
                LastName = "testuser12",
                Email = "testuser12@mail.com",
                UserName = "testuser12",
                Id = "testuser12"
            };

            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("User"));

            await userManager.CreateAsync(admin, "Admin12-");
            await userManager.AddToRoleAsync(admin, "Administrator");

            await userManager.CreateAsync(testuser, "Testuser-1");
            await userManager.AddToRoleAsync(testuser, "User");

            await userManager.CreateAsync(testuser2, "Testuser-2");
            await userManager.AddToRoleAsync(testuser2, "User");

            await userManager.CreateAsync(testuser3, "Testuser-3");
            await userManager.AddToRoleAsync(testuser3, "User");

            await userManager.CreateAsync(testuser4, "Testuser-4");
            await userManager.AddToRoleAsync(testuser4, "User");

            await userManager.CreateAsync(testuser5, "Testuser-5");
            await userManager.AddToRoleAsync(testuser5, "User");

            await userManager.CreateAsync(testuser6, "Testuser-6");
            await userManager.AddToRoleAsync(testuser6, "User");

            await userManager.CreateAsync(testuser7, "Testuser-7");
            await userManager.AddToRoleAsync(testuser7, "User");

            await userManager.CreateAsync(testuser8, "Testuser-8");
            await userManager.AddToRoleAsync(testuser8, "User");

            await userManager.CreateAsync(testuser9, "Testuser-9");
            await userManager.AddToRoleAsync(testuser9, "User");

            await userManager.CreateAsync(testuser10, "Testuser-10");
            await userManager.AddToRoleAsync(testuser10, "User");

            await userManager.CreateAsync(testuser11, "Testuser-11");
            await userManager.AddToRoleAsync(testuser11, "User");

            await userManager.CreateAsync(testuser12, "Testuser-12");
            await userManager.AddToRoleAsync(testuser12, "User");

        }


        private static void SeedBlogPosts(ApplicationContext context)
        {
            if (context.BlogPosts.Any())
            {
                return;   // DB has been seeded
            }

            context.BlogPosts.AddRange(
                new BlogPost
                {
                    Title = "This post was written by testuser2",
                    Content = "Only testuser2 and admin can modify this post. You need to login to comment to this post. \n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },

                new BlogPost
                {
                    Title = "This post was written by an admin",
                    Content = "Only the admin can modify this post \n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "admin"
                },

                new BlogPost
                {
                    Title = "Post3",
                    Content = "This is a SPECIAL STRING for search testing. You can search by title, by user or by content. \n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },

                new BlogPost
                {
                    Title = "Post4",
                    Content = "SPECIAL STRING for search testing \n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },
                new BlogPost
                {
                    Title = "Post4",
                    Content = "You can upload images to a post and see here the preview \n\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },
                new BlogPost
                {
                    Title = "Post6",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },
                new BlogPost
                {
                    Title = "Post7",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    CreatedAt = DateTime.Parse("1989-2-12"),
                    ModifiedAt = DateTime.Parse("1989-2-12"),
                    UserID = "testuser"
                },
                 new BlogPost
                 {
                     Title = "Post8",
                     Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                     CreatedAt = DateTime.Parse("1989-2-12"),
                     ModifiedAt = DateTime.Parse("1989-2-12"),
                     UserID = "testuser"
                 },
                  new BlogPost
                  {
                      Title = "Post9",
                      Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                      CreatedAt = DateTime.Parse("1989-2-12"),
                      ModifiedAt = DateTime.Parse("1989-2-12"),
                      UserID = "testuser"
                  },
                   new BlogPost
                   {
                       Title = "Post10",
                       Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                       CreatedAt = DateTime.Parse("1989-2-12"),
                       ModifiedAt = DateTime.Parse("1989-2-12"),
                       UserID = "testuser"
                   },
                    new BlogPost
                    {
                        Title = "Post11",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        CreatedAt = DateTime.Parse("1989-2-12"),
                        ModifiedAt = DateTime.Parse("1989-2-12"),
                        UserID = "testuser"
                    },
                     new BlogPost
                     {
                         Title = "Post12",
                         Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras efficitur turpis in porttitor commodo. Etiam facilisis, eros euismod sagittis lacinia, justo libero tempor ligula, interdum sodales dui eros ut ipsum. Cras ac vestibulum tellus. Fusce sed libero ligula. Duis dapibus lorem vel mauris commodo suscipit. Curabitur in venenatis dolor, sit amet vestibulum sapien. Nam mauris ligula, laoreet in lobortis ac, faucibus ut mi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer dignissim tortor a libero accumsan venenatis. Nullam ullamcorper urna in purus feugiat, vitae blandit diam consequat. Quisque fermentum eu nisl at convallis. Proin feugiat tortor nibh, accumsan scelerisque ipsum viverra eu. Proin id tortor eleifend lorem porta consequat ut eget nisl. Sed vitae commodo lectus. Aliquam ac eros eget libero venenatis mattis in eu risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                         CreatedAt = DateTime.Parse("1989-2-12"),
                         ModifiedAt = DateTime.Parse("1989-2-12"),
                         UserID = "testuser"
                     }
            );;
        }
    }
}
