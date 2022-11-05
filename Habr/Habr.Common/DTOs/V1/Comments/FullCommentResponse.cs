namespace Habr.Common.DTOs.V1.Comments;

public class FullCommentResponse
{
    public FullCommentResponse()
    {
        ChildComments = new List<FullCommentResponse>();
    }

    public int Id { get; set; }
    public string AuthorName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<FullCommentResponse> ChildComments { get; set; }
}