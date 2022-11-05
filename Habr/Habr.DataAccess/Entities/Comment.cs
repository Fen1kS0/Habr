namespace Habr.DataAccess.Entities;

public class Comment : BaseEntity
{
    public Comment()
    {
        Comments = new List<Comment>();
    }
    
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Comments { get; set; }
}