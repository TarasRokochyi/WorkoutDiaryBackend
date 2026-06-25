using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using BLL.DTO.ChartDTO;
using BLL.Services.Contracts;
using DAL.Specification;
using DAL.UOW;

namespace BLL.Services;

public class ChartService : IChartService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public ChartService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        //IMemoryCache cache
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_cache = cache;
    }

    public async Task<IEnumerable<WorkoutExerciseVolumeDTO>> getTotalVolumeAsync(int userId)
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdWithExercisesAsync(userId);
        workouts.OrderBy(t => t.Date);

        var workoutExercisesVolume = new List<WorkoutExerciseVolumeDTO>();
        foreach (var workout in workouts)
        {
            foreach (var exercise in workout.WorkoutExercises)
            {
                var ex = _mapper.Map<WorkoutExerciseVolumeDTO>(exercise);
                ex.Date = workout.Date;
                ex.Name = exercise.Exercise.Name;
                workoutExercisesVolume.Add(ex);
            }
        }
        return workoutExercisesVolume;
    }

    public async Task<IEnumerable<WorkoutExerciseMaxWeightDTO>> getMaxWeightAsync(int userId)
    {
        var wokoutExercisesMaxWeight = await _unitOfWork.WorkoutRepository.GetExercisesMaxWeight(userId);
        
        var result = _mapper.Map<IEnumerable<WorkoutExerciseMaxWeightDTO>>(wokoutExercisesMaxWeight);
        
        return result;
        //var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdWithExercisesAsync(userId);
        //workouts.OrderBy(t => t.Date);

        //foreach (var workout in workouts)
        //{
        //    var ex = workout.WorkoutExercises.GroupBy(e => e.ExerciseId).Select(g => g.Max(e => e.Exercise.Category == "strength" ? e.Weight : e.Distance));
        //}
        //var workoutExercisesMaxWeight = _mapper.Map<IEnumerable<WorkoutExerciseMaxWeightDTO>>(workouts);
        //
        //return workoutExercisesMaxWeight;
    }

    public async Task<KPIsDTO> getKPIsAsync(int userId, int period)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-period);
        var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdWithExercisesAsync(userId);
        var periodWorkouts = workouts.Where(w => w.Date >= cutoffDate).ToList();

        var kpisDTO = new KPIsDTO();
        kpisDTO.WorkoutsCompleted = periodWorkouts.Count;
        kpisDTO.TotalVolume = periodWorkouts
            .SelectMany(w => w.WorkoutExercises)
            .Sum(e => (double)((e.Sets ?? 0) * (e.Reps ?? 0) * (double)(e.Weight ?? 0)));
        kpisDTO.TrainingTimeHours = periodWorkouts.Sum(w => w.Duration ?? 0) / 60.0;
        kpisDTO.AverageWorkoutsPerWeek = period > 0 ? kpisDTO.WorkoutsCompleted / (period / 7.0) : 0;
        kpisDTO.TopExercise = periodWorkouts
            .SelectMany(w => w.WorkoutExercises)
            .Where(e => e.Exercise?.Name != null)
            .GroupBy(e => e.Exercise!.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "—";

        return kpisDTO;
    }

    public async Task<IEnumerable<ExerciseDistributionDTO>> getExerciseDistributionAsync(int userId, int period)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-period);
        var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdWithExercisesAsync(userId);

        var result = workouts
            .Where(w => w.Date >= cutoffDate)
            .SelectMany(w => w.WorkoutExercises)
            .Where(e => e.Exercise?.Name != null)
            .GroupBy(e => e.Exercise!.Name)
            .Select(g => new ExerciseDistributionDTO { Name = g.Key, Count = g.Count() })
            .OrderByDescending(e => e.Count)
            .Take(5)
            .ToList();

        return result;
    }

    public async Task<IEnumerable<HeatmapDayDTO>> getHeatmapAsync(int userId)
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdAsync(userId);

        var result = workouts
            .Where(w => w.Date.HasValue)
            .GroupBy(w => w.Date!.Value.Date)
            .Select(g => new HeatmapDayDTO
            {
                Date = g.Key,
                TotalDurationMinutes = g.Sum(w => w.Duration ?? 0)
            })
            .OrderBy(d => d.Date)
            .ToList();

        return result;
    }
}