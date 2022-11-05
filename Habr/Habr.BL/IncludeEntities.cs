using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Habr.BL;

public static class IncludeEntities
{
    public static IIncludableQueryable<Post, object> IncludeAllForPublicPost(IQueryable<Post> query)
    {
        return query
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .ThenInclude(c => c.Author);
    }
}