namespace BLL.DTO;

public class UserRequestDTO
{
    public string? Name { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public string? Level { get; set; }

    public string? Gender { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public int? Age { get; set; }
    
    // account info
    public string? Email { get; set; }
    
    public string UserName { get; set; }
}