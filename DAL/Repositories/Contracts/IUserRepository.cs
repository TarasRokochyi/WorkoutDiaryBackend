using DAL.Models;
using DAL.Models.Entities;

namespace DAL.Repositories.Contracts;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByTokenAsync(String token);
}