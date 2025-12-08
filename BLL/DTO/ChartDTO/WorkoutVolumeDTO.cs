namespace BLL.DTO.ChartDTO;

public class WorkoutVolumeDTO
{
    public int WorkoutId { get; set; }
    
    public string? Name { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<WorkoutExerciseResponseDTO> WorkoutExercises { get; set; } = new List<WorkoutExerciseResponseDTO>();
}