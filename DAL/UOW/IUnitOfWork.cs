using DAL.Repositories;
using DAL.Repositories.Contracts;

namespace DAL.UOW;

public interface IUnitOfWork
{
    IExerciseRepository ExerciseRepository { get; }
    IUserRepository UserRepository { get; }
    IWorkoutExerciseRepository WorkoutExerciseRepository { get; }
    IWorkoutRepository WorkoutRepository { get; }
    IWorkoutTemplateRepository WorkoutTemplateRepository { get; }
    IEquipmentRepository EquipmentRepository { get; }

    Task<int> CompleteAsync(CancellationToken cancellationToken = default(CancellationToken));
    
}