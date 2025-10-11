namespace DAL.Models;
public class WorkoutTemplate
{
    public int TemplateId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public int? Duration { get; set; }

    public string? Notes { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

}
