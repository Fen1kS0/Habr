using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Habr.DataAccess.Repositories;

public class RatingRepository : GenericRepository<Rating>, IRatingRepository
{
    public RatingRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<Rating?> GetRatingByPostAndUser(int postId, int userId)
    {
        return await DataContext.Ratings
            .SingleOrDefaultAsync(r => r.PostId == postId && r.UserId == userId);
    }
}