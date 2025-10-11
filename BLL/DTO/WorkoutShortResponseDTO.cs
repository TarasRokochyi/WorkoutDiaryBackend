namespace BLL.DTO;

public class WorkoutShortResponseDTO
{
    
    public int WorkoutId { get; set; }
    
    public int UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? Date { get; set; }

    public int? Duration { get; set; }

    public string? Notes { get; set; }
}