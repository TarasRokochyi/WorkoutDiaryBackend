namespace DAL.Models.Entities;

public class Equipment
{
    public int EquipmentId { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}