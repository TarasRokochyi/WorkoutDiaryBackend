using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using DAL.Repositories.Contracts;

namespace DAL.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly WorkoutDiaryContext _context;
    public IExerciseRepository ExerciseRepository { get; }
    public IUserRepository UserRepository { get; }
    public IWorkoutExerciseRepository WorkoutExerciseRepository { get; }
    public IWorkoutRepository WorkoutRepository { get; }
    public IWorkoutTemplateRepository WorkoutTemplateRepository { get; }
    public IEquipmentRepository EquipmentRepository { get; }

    public UnitOfWork(WorkoutDiaryContext context,
        IUserRepository userRepository,
        IExerciseRepository exerciseRepository,
        IWorkoutExerciseRepository workoutExerciseRepository,
        IWorkoutRepository workoutRepository,
        IWorkoutTemplateRepository workoutTemplateRepository,
        IEquipmentRepository equipmentRepository)
    {
        _context = context;
        ExerciseRepository = exerciseRepository;
        UserRepository = userRepository;
        WorkoutExerciseRepository = workoutExerciseRepository;
        WorkoutRepository = workoutRepository;
        WorkoutTemplateRepository = workoutTemplateRepository;
        EquipmentRepository = equipmentRepository;
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}