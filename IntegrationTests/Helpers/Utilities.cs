using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.DAL;
using System;
using System.Collections.Generic;

namespace BlogApp.Dotnet.Web.IntegrationTests.Helpers
{
    public class Utilities
    {
        public static void InitializeDbForTests(ApplicationContext db)
        {
            db.BlogPosts.AddRange(GetSeedingPosts());
            db.Comments.AddRange(GetSeedingComments());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationContext db)
        {
            db.BlogPosts.RemoveRange(db.BlogPosts);
            db.Comments.RemoveRange(db.Comments);
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }

        public static List<BlogPost> GetSeedingPosts()
        {
            return new List<BlogPost>() {
                new BlogPost(){ Title = "TestTitle1",
                        ID = 1,
                        Content = "TestContent1",
                        CreatedAt = DateTime.Parse("01-01-2001"),
                        ModifiedAt = DateTime.Parse("01-01-2001"),
                        ImageURL = "/Assets/Uploads/1.jpg",
                        UserID = "testuser2"},

                new BlogPost(){ Title = "TestTitle2",
                        ID = 2,
                        Content = "TestContent2",
                        CreatedAt = DateTime.Parse("02-01-2001"),
                        ModifiedAt = DateTime.Parse("02-01-2001"),
                        ImageURL = "/Assets/Uploads/2.jpg",
                        UserID = "testuser"},

                 new BlogPost(){ Title = "TestTitle3",
                        ID = 3,
                        Content = "TestContent3",
                        CreatedAt = DateTime.Parse("03-01-2001"),
                        ModifiedAt = DateTime.Parse("03-01-2001"),
                        ImageURL = "/Assets/Uploads/3.jpg",
                        UserID = "testuser"},

                 new BlogPost(){ Title = "TestTitle4",
                        ID = 4,
                        Content = "TestContent4",
                        CreatedAt = DateTime.Parse("03-01-2001"),
                        ModifiedAt = DateTime.Parse("03-01-2001"),
                        ImageURL = "/Assets/Uploads/4.jpg",
                        UserID = "testuser"},

                 new BlogPost(){ Title = "TestTitle5",
                        ID = 5,
                        Content = "TestContent5",
                        CreatedAt = DateTime.Parse("03-01-2001"),
                        ModifiedAt = DateTime.Parse("03-01-2001"),
                        ImageURL = "/Assets/Uploads/5.jpg",
                        UserID = "testuser"},
                 new BlogPost(){ Title = "TestTitle6",
                        ID = 6,
                        Content = "TestContent6",
                        CreatedAt = DateTime.Parse("03-01-2001"),
                        ModifiedAt = DateTime.Parse("03-01-2001"),
                        ImageURL = "/Assets/Uploads/6.jpg",
                        UserID = "testuser"},
                 new BlogPost(){ Title = "TestTitle7",
                        ID = 7,
                        Content = "TestContent7",
                        CreatedAt = DateTime.Parse("03-01-2001"),
                        ModifiedAt = DateTime.Parse("03-01-2001"),
                        ImageURL = "/Assets/Uploads/7.jpg",
                        UserID = "testuser"},
                 new BlogPost(){ Title = "TestTitle8",
                        ID = 8,
                        Content = "TestContent8",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/8.jpg",
                        UserID = "testuser"},
                 new BlogPost(){ Title = "TestTitle9",
                        ID = 9,
                        Content = "TestContent9",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/9.jpg",
                        UserID = "testuser"},
                  new BlogPost(){ Title = "TestTitle10",
                        ID = 10,
                        Content = "TestContent10",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/10.jpg",
                        UserID = "testuser"},
                   new BlogPost(){ Title = "TestTitle11",
                        ID = 11,
                        Content = "TestContent11",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/11.jpg",
                        UserID = "testuser"},
                    new BlogPost(){ Title = "TestTitle12",
                        ID = 12,
                        Content = "TestContent12",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/12.jpg",
                        UserID = "testuser"},
                     new BlogPost(){ Title = "TestTitle13",
                        ID = 13,
                        Content = "TestContent13",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/13.jpg",
                        UserID = "testuser"},
                     new BlogPost(){ Title = "TestTitle14",
                        ID = 14,
                        Content = "TestContent14",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/14.jpg",
                        UserID = "testuser"},
                     new BlogPost(){ Title = "TestTitle15",
                        ID = 15,
                        Content = "TestContent15",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/15.jpg",
                        UserID = "testuser"},
                     new BlogPost(){ Title = "TestTitle16",
                        ID = 16,
                        Content = "TestContent16",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/16.jpg",
                        UserID = "testuser"},
                     new BlogPost(){ Title = "TestTitle17",
                        ID = 17,
                        Content = "TestContent17",
                        CreatedAt = DateTime.Parse("03-01-2002"),
                        ModifiedAt = DateTime.Parse("03-01-2002"),
                        ImageURL = "/Assets/Uploads/17.jpg",
                        UserID = "testuser"},
            };
        }

        public static List<Comment> GetSeedingComments()
        {
            return new List<Comment>() {
                new Comment(){
                    ID = 1,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },

                new Comment(){
                    ID = 2,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                 new Comment(){
                    ID = 3,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },

                 new Comment(){
                    ID = 4,
                    ParentID = 3,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                 new Comment(){
                    ID = 5,
                    ParentID = 3,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                 new Comment(){
                    ID = 6,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                 new Comment(){
                    ID = 7,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                 new Comment(){
                    ID = 8,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                new Comment(){
                    ID = 9,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                new Comment(){
                    ID = 10,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                new Comment(){
                    ID = 11,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser2",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                },
                new Comment(){
                    ID = 12,
                    ParentID = null,
                    PostID = 1,
                    UserID = "testuser2",
                    Date = DateTime.Now,
                    Content = "This is a seeding comment."
                }
            };
        }
    }
}
