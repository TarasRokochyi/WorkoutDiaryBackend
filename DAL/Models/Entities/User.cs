using Microsoft.AspNetCore.Identity;

namespace DAL.Models.Entities;
public partial class User : IdentityUser<int>
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Level { get; set; }

    public string? Gender { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public int? Age { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();

    public virtual ICollection<WorkoutTemplate> WorkoutTemplates { get; set; } = new List<WorkoutTemplate>();
    public List<RefreshToken> RefreshTokens { get; set; }
}
