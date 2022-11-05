using Habr.Common.DTOs.V2.Posts;

namespace Habr.BL.Interfaces.Services.V2;

public interface IPostServiceV2
{
    Task<IEnumerable<PostResponseV2>> GetAllPostsAsync();
    
    Task<IEnumerable<FullPostResponseV2>> GetAllPublicPostsAsync();
    Task<IEnumerable<FullPostResponseV2>> GetPublicPostsByUserAsync(int authorId);
    
    Task<IEnumerable<PostResponseV2>> GetAllDraftPostsAsync();
    Task<IEnumerable<PostResponseV2>> GetDraftPostsByUserAsync(int authorId);
    
    Task<FullPostResponseV2> GetPostByIdAsync(int id);
}