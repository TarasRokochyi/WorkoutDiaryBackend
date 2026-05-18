using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(WorkoutDiaryContext context) : base(context)
    {
    }

    public async Task<User> GetUserByTokenAsync(String token)
    {
        var user = await table.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        return user;
    }
}