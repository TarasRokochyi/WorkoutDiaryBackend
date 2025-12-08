using AutoMapper;
using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Models.Entities;
using DAL.UOW;

namespace BLL.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly IMemoryCache _cache;
    

    public WorkoutService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        //IMemoryCache cache
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_cache = cache;
    }
    
    public async Task<IEnumerable<WorkoutResponseDTO>> GetAllWorkoutsAsync()
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<WorkoutResponseDTO>>(workouts);
        return result;
    }

    public async Task<IEnumerable<WorkoutShortResponseDTO>> GetAllWorkoutsByUserIdAsync(int userId)
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetByUserIdAsync(userId);
        var result = _mapper.Map<IEnumerable<WorkoutShortResponseDTO>>(workouts);
        return result;
    }

    public async Task<WorkoutResponseDTO> GetWorkoutByIdAsync(int id)
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetByIdAsync(id);
        var result = _mapper.Map<WorkoutResponseDTO>(workouts);
        return result;
    }
    
    public async Task<WorkoutResponseDTO> GetUserWorkoutByIdAsync(int userId, int id)
    {
        var workouts = await _unitOfWork.WorkoutRepository.GetUserWorkoutAsync(userId, id);
        var result = _mapper.Map<WorkoutResponseDTO>(workouts);
        return result;
    }

    public async Task<WorkoutResponseDTO> AddWorkoutAsync(WorkoutRequestDTO workout)
    {
        var workoutToAdd = _mapper.Map<Workout>(workout);
        var addedWorkout = await _unitOfWork.WorkoutRepository.AddAsync(workoutToAdd);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<WorkoutResponseDTO>(addedWorkout);
        return result;
    }

    public async Task<WorkoutResponseDTO> UpdateUserWorkoutAsync(int userId, int id, WorkoutRequestDTO workout)
    {
        var workoutToUpdate = await _unitOfWork.WorkoutRepository.GetUserWorkoutAsync(userId, id);
        if (workoutToUpdate == null)
        {
            throw new Exception("Not found.");
        }
        
        _mapper.Map(workout, workoutToUpdate);
        await _unitOfWork.WorkoutRepository.UpdateAsync(workoutToUpdate);
        await _unitOfWork.CompleteAsync();
        
        var updatedWorkout = await _unitOfWork.WorkoutRepository.GetUserWorkoutAsync(userId, id);
        
        var result = _mapper.Map<WorkoutResponseDTO>(updatedWorkout);
        return result;
    }

    public async Task DeleteWorkoutAsync(int id)
    {
        var workoutToDelete = await _unitOfWork.WorkoutRepository.GetByIdAsync(id);
        if (workoutToDelete != null)
        {
            throw new Exception("Not Found");
        }
        await _unitOfWork.WorkoutRepository.DeleteAsync(workoutToDelete);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteUserWorkoutAsync(int userId, int id)
    {
        var workoutToDelete = await _unitOfWork.WorkoutRepository.GetUserWorkoutAsync(userId, id);
        if (workoutToDelete == null)
        {
            throw new Exception("Not Found");
        }
        await _unitOfWork.WorkoutRepository.DeleteAsync(workoutToDelete);
        await _unitOfWork.CompleteAsync();
    }
}