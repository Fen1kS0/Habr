namespace Habr.Common.DTOs.V1.Comments
{
    public class AddCommentRequest
    {
        public string Text { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
