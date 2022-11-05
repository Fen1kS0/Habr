using Habr.Common;
using Habr.DataAccess.Entities;

namespace Habr.DataAccess;

public static class SeedData
{
    public static void Seed(DataContext db)
    {
        db.Database.EnsureCreated();
        
        if (!db.Users.Any())
        {
            var users = new List<User>()
            {
                new User()
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Password = Hash.HashPassword("123456"),
                    Role = RoleNames.Administrator
                },
                new User()
                {
                    Name = "Alex",
                    Email = "test@gmail.com",
                    Password = Hash.HashPassword("123456"),
                    Role = RoleNames.User
                },
                new User()
                {
                    Name = "Rik",
                    Email = "rik12@ya.ru",
                    Password = Hash.HashPassword("654321"),
                    Role = RoleNames.User
                }
            };
                
            db.Users.AddRange(users);
            db.SaveChanges();
        }
            
        if (!db.Posts.Any())
        {
            var posts = new List<Post>()
            {
                new Post()
                {
                    Author = db.Users.First(),
                    Title = "How create migration?",
                    Text = "Use command: dotnet ef migrations add {nameMigration}",
                    IsPublished = true
                },
                new Post()
                {
                    Author = db.Users.ToList().Last(),
                    Title = "How to find the greatest common divisor?",
                    Text = "Euclid's algorithm",
                    IsPublished = true
                }
            };
                
            db.Posts.AddRange(posts);
            db.SaveChanges();
        }
            
        if (!db.Comments.Any())
        {
            var comments = new List<Comment>()
            {
                new Comment()
                {
                    Author = db.Users.ToList().Last(),
                    Post = db.Posts.First(),
                    Text = "Thanks, bro!",
                },
                    
                new Comment()
                {
                    Author = db.Users.First(),
                    Post = db.Posts.ToList().Last(),
                    Text = "It interesting ^_^"
                },
            };
                
            db.Comments.AddRange(comments);
            db.SaveChanges();
        
            db.Comments.Add(new Comment()
            {
                Author = db.Users.First(),
                Post = db.Posts.First(),
                ParentComment = db.Comments.First(),
                Text = "No problem)"
            });
            db.SaveChanges();
        }

        if (!db.Ratings.Any())
        {
            var ratings = new List<Rating>()
            {
                new Rating()
                {
                    Value = 5,
                    Post = db.Posts.First(),
                    User = db.Users.First(),
                },
                    
                new Rating()
                {
                    Value = 3,
                    Post = db.Posts.First(),
                    User = db.Users.Skip(1).First(),
                },
            };
                
            db.Ratings.AddRange(ratings);
            db.SaveChanges();
        }
    }
}