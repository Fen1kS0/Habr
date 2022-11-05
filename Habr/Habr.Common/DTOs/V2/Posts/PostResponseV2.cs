using Habr.Common.DTOs.V1.Users;

namespace Habr.Common.DTOs.V2.Posts;

public class PostResponseV2
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public UserResponse Author { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public decimal Rating { get; set; }
}