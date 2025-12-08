using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;

namespace DAL.Repositories;

public class WorkoutExerciseRepository : GenericRepository<WorkoutExercise>, IWorkoutExerciseRepository
{
    public WorkoutExerciseRepository(GymWebServiceContext context) : base(context)
    {
    }
}