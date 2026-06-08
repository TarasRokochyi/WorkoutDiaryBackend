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

    public async Task<List<ExerciseRecommendation>> GetExercisesByEquipmentNameList(List<string> detectedEquipmentNames, string? difficulty = null)
    {
        var query = table
            .Where(eq => detectedEquipmentNames.Contains(eq.Name))
            .SelectMany(eq => eq.Exercises.Select(ex => new ExerciseRecommendation
            {
                EquipmentName = eq.Name,
                Exercise = ex
            }));

        if (!string.IsNullOrWhiteSpace(difficulty))
            query = query.Where(r => r.Exercise.Difficulty == difficulty);

        var result = await query.ToListAsync();
        
        // var dict = result.ToDictionary(
        //     x => x.Equipment,     // key = equipment name
        //     x => x.Exercises      // value = List<Exercise>
        // );

        return result;
    }

    public async Task<List<string>> GetAllEquipmentNamesAsync()
    {
        return await table.Select(e => e.Name).OrderBy(n => n).ToListAsync();
    }
}