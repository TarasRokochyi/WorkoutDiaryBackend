using DAL.Models;
using DAL.Models.Entities;

namespace DAL.Repositories.Contracts;

public interface IWorkoutRepository : IGenericRepository<Workout>
{
    Task<IEnumerable<WorkoutExerciseMaxWeightChart>> GetExercisesMaxWeight(int userId);
    Task<IEnumerable<Workout>> GetByUserIdWithExercisesAsync(int userId);
    Task<IEnumerable<Workout>> GetByUserIdAsync(int userId);
    Task<Workout> GetUserWorkoutAsync(int userId, int id);
    
}
