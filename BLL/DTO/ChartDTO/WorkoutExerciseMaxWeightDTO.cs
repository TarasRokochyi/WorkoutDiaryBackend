namespace BLL.DTO.ChartDTO;

public class WorkoutExerciseMaxWeightDTO
{
    public int? WorkoutId { get; set; }

    public int? ExerciseId { get; set; }
    public int? MaxWeight { get; set; }
    public DateTime? Date { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
}