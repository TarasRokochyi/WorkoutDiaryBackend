using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class WorkoutTemplateRepository : GenericRepository<WorkoutTemplate>, IWorkoutTemplateRepository
{
    public WorkoutTemplateRepository(GymWebServiceContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<WorkoutTemplate>> GetAllAsync()
    {
        var result = await table.Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<WorkoutTemplate>> GetByUserId(int userId)
    {
        var result = await table.Where(t => t.UserId == userId || t.UserId == null).Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).ToListAsync();
        return result;
    }

    public async Task<WorkoutTemplate> GetUserTemplateAsync(int userId, int id)
    {
        var result = await table.Where(t => t.UserId == userId || t.TemplateId == id).Include(w => w.WorkoutExercises).ThenInclude(w => w.Exercise).FirstOrDefaultAsync();
        return result;
    }
}