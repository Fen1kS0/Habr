using Habr.DataAccess.Entities;

namespace Habr.DataAccess.Interfaces.Repositories;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<IReadOnlyCollection<Post>> GetPostsByUserAsync(int userId);
    Task LoadCommentsAsync(Post post);
    Task<IEnumerable<int>> GetPostsId();
}