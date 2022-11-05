using Habr.DataAccess.Entities;

namespace Habr.DataAccess.Interfaces.Repositories;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task<Rating?> GetRatingByPostAndUser(int postId, int userId);
}