namespace Habr.Common.DTOs.V1.Posts;

public class PostAddedResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public int AuthorId { get; set; }
}