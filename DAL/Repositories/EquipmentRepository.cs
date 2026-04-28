using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(WorkoutDiaryContext context) : base(context)
    {
    }

    public async Task<List<ExerciseRecommendation>> GetExercisesByEquipmentNameList(List<string> detectedEquipmentNames)
    {
        var result = await table
            .Where(eq => detectedEquipmentNames.Contains(eq.Name))
            .SelectMany(eq => eq.Exercises.Select(ex => new ExerciseRecommendation
            {
                EquipmentName = eq.Name,
                Exercise = ex
            }))
            .ToListAsync();
        
        // var dict = result.ToDictionary(
        //     x => x.Equipment,     // key = equipment name
        //     x => x.Exercises      // value = List<Exercise>
        // );

        return result;
    }
}