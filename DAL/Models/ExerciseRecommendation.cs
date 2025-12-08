using DAL.Models.Entities;

namespace DAL.Models;

public class ExerciseRecommendation
{
    public string EquipmentName { get; set; }
    public Exercise Exercise { get; set; }
}