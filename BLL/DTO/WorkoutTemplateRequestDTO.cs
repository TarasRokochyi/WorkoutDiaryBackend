namespace BLL.DTO;

public class WorkoutTemplateRequestDTO
{
    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<WorkoutExerciseRequestDTO> WorkoutExercises { get; set; } = new List<WorkoutExerciseRequestDTO>();
}