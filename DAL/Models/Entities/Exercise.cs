namespace DAL.Models.Entities;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? MuscleGroups { get; set; }

    public string? Description { get; set; }

    public string? Difficulty { get; set; }

    public virtual User? User { get; set; }
    public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

    public virtual ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
