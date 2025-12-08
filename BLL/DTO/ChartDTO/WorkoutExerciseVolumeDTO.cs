using System.Runtime.InteropServices.JavaScript;

namespace BLL.DTO.ChartDTO;

public class WorkoutExerciseVolumeDTO
{
    public int WorkoutExerciseId { get; set; }

    public int? WorkoutId { get; set; }

    public int? ExerciseId { get; set; }
    
    public int? Volume { get; set; }
    
    public DateTime? Date { get; set; }
    
    public string Name { get; set; }

}