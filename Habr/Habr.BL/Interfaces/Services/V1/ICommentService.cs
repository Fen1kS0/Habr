using Habr.Common.DTOs.V1.Comments;

namespace Habr.BL.Interfaces.Services.V1;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetCommentsAsync(int postId);
    Task<IEnumerable<CommentResponse>> GetCommentsByUserAsync(int postId, int authorId);
    Task<CommentResponse> GetCommentsByIdAsync(int postId, int id);
    Task<CommentAddedResponse> AddCommentAsync(int postId, AddCommentRequest addCommentRequest, int authorId);
    Task UpdateCommentAsync(int postId, int id, UpdateCommentRequest updateCommentRequest, int authorId);
    Task UpdateCommentAsync(int postId, int id, UpdateCommentRequest updateCommentRequest);
    Task DeleteCommentAsync(int postId, int id, int authorId);
    Task DeleteCommentAsync(int postId, int id);
}