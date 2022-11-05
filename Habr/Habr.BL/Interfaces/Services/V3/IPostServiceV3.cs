using Habr.Common.DTOs.Pages;
using Habr.Common.DTOs.V2.Posts;

namespace Habr.BL.Interfaces.Services.V3;

public interface IPostServiceV3
{
    Task<PagedResponse<PostResponseV2>> GetAllPostsAsync(PageParameters pageParameters);
    
    Task<PagedResponse<FullPostResponseV2>> GetAllPublicPostsAsync(PageParameters pageParameters);
    Task<PagedResponse<FullPostResponseV2>> GetPublicPostsByUserAsync(int authorId, PageParameters pageParameters);
    
    Task<PagedResponse<PostResponseV2>> GetAllDraftPostsAsync(PageParameters pageParameters);
    Task<PagedResponse<PostResponseV2>> GetDraftPostsByUserAsync(int authorId, PageParameters pageParameters);
}