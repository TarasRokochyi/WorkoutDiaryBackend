using DAL.Models;
using DAL.Models.Entities;

namespace DAL.Repositories.Contracts;

public interface IEquipmentRepository : IGenericRepository<Equipment>
{

    Task<List<ExerciseRecommendation>> GetExercisesByEquipmentNameList(List<string> detectedEquipmentNames, string? difficulty = null);
    Task<List<string>> GetAllEquipmentNamesAsync();
}