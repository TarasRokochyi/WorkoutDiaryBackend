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
        var date = DateTime.UtcNow.AddDays(-period);
        var periodSpec = new WorkoutPeriodSpecification(x => x.UserId == userId && x.Date >= date );

        var result = await _unitOfWork.WorkoutRepository.FindWithSpecificationAsync(periodSpec);
        
        var kpisDTO = new KPIsDTO();
        // kpisDTO.TotalVolume = ;
        // kpisDTO.WorkoutsCompleted = ;
        // kpisDTO.TrainingTimeHours = ;
        // kpisDTO.AverageWorkoutsPerWeek = ;
        
        
        return kpisDTO;
    }
}