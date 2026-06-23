namespace BLL.DTO;

public class ManualRecommendationRequestDTO
{
    public List<string> EquipmentNames { get; set; } = new();
    public string? Difficulty { get; set; }
}
