using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Habr.DataAccess.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await DataContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }
}