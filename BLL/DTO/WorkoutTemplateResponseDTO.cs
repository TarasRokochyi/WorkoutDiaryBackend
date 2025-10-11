namespace BLL.DTO;

public class WorkoutTemplateResponseDTO
{
    public int TemplateId { get; set; }
    
    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<WorkoutExerciseResponseDTO> WorkoutExercises { get; set; } = new List<WorkoutExerciseResponseDTO>();
}