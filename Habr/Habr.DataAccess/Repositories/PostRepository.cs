using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Habr.DataAccess.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(DataContext dataContext) : base(dataContext)
    {
    }
    
    public override async Task UpdateEntityAsync(Post entity)
    {
        entity.UpdatedAt = DateTime.Now;
        await base.UpdateEntityAsync(entity);
    }
    
    public async Task<IReadOnlyCollection<Post>> GetPostsByUserAsync(int userId)
    {
        return await DataContext.Posts.Where(dp => dp.AuthorId == userId).ToListAsync();
    }
    
    public async Task LoadCommentsAsync(Post publicPost)
    {
        await DataContext.Entry(publicPost).Collection(p => p.Comments).LoadAsync();
    }

    public async Task<IEnumerable<int>> GetPostsId()
    {
        var postsId = await DataContext.Posts.Select(p => p.Id).ToListAsync();

        return postsId;
    }
}