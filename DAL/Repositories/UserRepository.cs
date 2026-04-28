using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;

namespace DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(WorkoutDiaryContext context) : base(context)
    {
    }
}