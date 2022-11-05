using Habr.DataAccess.Entities;

namespace Habr.DataAccess.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
}