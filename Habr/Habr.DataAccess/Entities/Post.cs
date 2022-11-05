namespace Habr.DataAccess.Entities;

public class Post : BaseEntity
{
    public Post()
    {
        Comments = new List<Comment>();
    }
    
    public string Title { get; set; }
    public string Text { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public decimal Rating { get; set; }
    public ICollection<Comment> Comments { get; set; }
}