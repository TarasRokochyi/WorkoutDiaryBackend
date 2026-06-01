using BLL.DTO.ChartDTO;

namespace BLL.Services.Contracts;

public interface IChartService
{
    Task<IEnumerable<WorkoutExerciseVolumeDTO>> getTotalVolumeAsync(int userId);
    Task<IEnumerable<WorkoutExerciseMaxWeightDTO>> getMaxWeightAsync(int userId);
    Task<KPIsDTO> getKPIsAsync(int userId, int period);
}