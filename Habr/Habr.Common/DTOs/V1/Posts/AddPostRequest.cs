namespace Habr.Common.DTOs.V1.Posts;

public class AddPostRequest
{
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsPublished { get; set; }
}