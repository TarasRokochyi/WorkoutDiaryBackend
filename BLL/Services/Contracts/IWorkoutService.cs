using BLL.DTO;

namespace BLL.Services.Contracts;

public interface IWorkoutService
{
    
    Task<IEnumerable<WorkoutResponseDTO>> GetAllWorkoutsAsync();
    Task<IEnumerable<WorkoutShortResponseDTO>> GetAllWorkoutsByUserIdAsync(int userId);
    Task<WorkoutResponseDTO> GetWorkoutByIdAsync(int id);
    Task<WorkoutResponseDTO> GetUserWorkoutByIdAsync(int userId, int id);
    Task<WorkoutResponseDTO> AddWorkoutAsync(WorkoutRequestDTO workout);
    Task<WorkoutResponseDTO> UpdateUserWorkoutAsync(int userId, int id, WorkoutRequestDTO workout);
    Task DeleteWorkoutAsync(int id);
    Task DeleteUserWorkoutAsync(int userId, int id);
}