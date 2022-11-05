using Habr.Common.DTOs.V1.Comments;
using Habr.Common.DTOs.V1.Users;

namespace Habr.Common.DTOs.V2.Posts;

public class FullPostResponseV2
{
    public FullPostResponseV2()
    {
        TreeComments = new List<FullCommentResponse>();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public UserResponse Author { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public decimal Rating { get; set; }
    public IEnumerable<FullCommentResponse> TreeComments { get; set; }
}