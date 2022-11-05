namespace Habr.DataAccess.Entities;

public class User : BaseEntity
{
    public User()
    {
        Posts = new List<Post>();
        Comments = new List<Comment>();
    }
    
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string Role { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Rating> Ratings { get; set; }
}