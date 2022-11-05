using Habr.Common.DTOs.V1.Posts;

namespace Habr.BL.Interfaces.Services.V1;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetAllPostsAsync();
    
    Task<IEnumerable<FullPostResponse>> GetAllPublicPostsAsync();
    Task<IEnumerable<FullPostResponse>> GetPublicPostsByUserAsync(int authorId);
    
    Task<IEnumerable<PostResponse>> GetAllDraftPostsAsync();
    Task<IEnumerable<PostResponse>> GetDraftPostsByUserAsync(int authorId);
    
    Task<FullPostResponse> GetPostByIdAsync(int id);
    Task<PostAddedResponse> AddPostAsync(AddPostRequest addPostRequest, int authorId);
    Task UpdatePostAsync(int id, UpdatePostRequest updatePostRequest, int authorId);
    Task UpdatePostAsync(int id, UpdatePostRequest updatePostRequest);
    Task DeletePostAsync(int id, int authorId);
    Task DeletePostAsync(int id);
    
    Task PublishPostAsync(int id, int authorId);
    Task MoveToDraftAsync(int id, int authorId);
}