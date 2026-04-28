using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class WorkoutRepository : GenericRepository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(WorkoutDiaryContext context) : base(context)
    {
    }

    public override async Task<Workout> GetByIdAsync(int id)
    {
        var result = await table.Where(w => w.WorkoutId == id).Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).FirstOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<Workout>> GetByUserIdWithExercisesAsync(int userId)
    {
        var result = await table.Where(t => t.UserId == userId)
            .Include(w => w.WorkoutExercises)
            .ThenInclude(w => w.Exercise)
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<WorkoutExerciseMaxWeightChart>> GetExercisesMaxWeight(int userId)
    {
         var result = await context
             .Database
             .SqlQuery<WorkoutExerciseMaxWeightChart>($"""
             SELECT
                w."workoutid",
                we."exerciseid",
                CASE 
                    WHEN e."category" = 'Strength' THEN Max(we."weight")
                    ELSE Max(we."distance")
                END AS "maxweight",
                w."date",
                e."name",
                e."category"
             FROM "workouts" AS w
             JOIN "workoutexercises" AS we
                 ON we."workoutid" = w."workoutid"
             JOIN "exercises" AS e
                 ON we."exerciseid" = e."exerciseid"
             WHERE w."userid" = {userId}
             Group by
             w."workoutid",
             we."exerciseid",
             e."category",
             e."name";
             """).ToListAsync();

        return result;
    }

    public  async Task<IEnumerable<Workout>> GetByUserIdAsync(int userId)
    {
        var result = await table.Where(t => t.UserId == userId).ToListAsync();
        return result;
    }

    public async Task<Workout> GetUserWorkoutAsync(int userId, int id)
    {
        var result = await table.Where(w => w.UserId == userId && w.WorkoutId == id).Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).FirstOrDefaultAsync();
        return result;
    }
}