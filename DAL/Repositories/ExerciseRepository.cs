using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(WorkoutDiaryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Exercise>> GetByUserIdAsync(int userId)
    {
        var result = await table.Where(e => e.UserId == userId || e.UserId == null).ToListAsync();
        return result;
    }
    
    public async Task<Exercise> GetUserExerciseAsync(int userId, int exerciseId)
    {
        var result = await table.Where(e => e.UserId == userId || e.ExerciseId == exerciseId).FirstOrDefaultAsync();
        return result;
    }
    
    
}