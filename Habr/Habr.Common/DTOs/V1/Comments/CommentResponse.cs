namespace Habr.Common.DTOs.V1.Comments;

public class CommentResponse
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string AuthorName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}