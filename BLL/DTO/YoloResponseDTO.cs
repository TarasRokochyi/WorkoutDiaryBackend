namespace BLL.DTO;

public class YoloResponseDTO
{
    public string image {get; set; }
    public List<YoloObjectDTO> objects {get; set; }
    public int count {get; set; }
    
}