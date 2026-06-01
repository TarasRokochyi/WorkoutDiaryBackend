using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using DAL.Specification;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly WorkoutDiaryContext context;
    protected readonly DbSet<T> table;

    public GenericRepository(WorkoutDiaryContext context)
    {
        this.context = context;
        table = context.Set<T>();
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await table.ToListAsync(); 
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await table.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException();
        }
        var addedEntity = await table.AddAsync(entity);
        return addedEntity.Entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException();
        }
        var updatedEntity = context.Update(entity);
        return await Task.Run(() => updatedEntity.Entity);
    }

    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        await Task.Run(() => table.Remove(entity));
    }

    public virtual async Task DeleteAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException();
        }
        await Task.Run(() => table.Remove(entity));
    }

    public async Task<IEnumerable<T>> FindWithSpecificationAsync(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(table.AsQueryable(), specification);
    }
    
}