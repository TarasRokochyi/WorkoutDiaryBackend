using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;

namespace DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(GymWebServiceContext context) : base(context)
    {
    }
}