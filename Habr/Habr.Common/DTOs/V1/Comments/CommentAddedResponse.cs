namespace Habr.Common.DTOs.V1.Comments;

public class CommentAddedResponse
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public int PostId { get; set; }
    public string Text { get; set; }
    public int? ParentCommentId { get; set; }
}