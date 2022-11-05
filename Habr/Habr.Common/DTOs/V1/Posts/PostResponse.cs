namespace Habr.Common.DTOs.V1.Posts;

public class PostResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string AuthorEmail { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public decimal Rating { get; set; }
}