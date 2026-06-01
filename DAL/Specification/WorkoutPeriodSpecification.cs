using System.Linq.Expressions;
using DAL.Models.Entities;

namespace DAL.Specification;

public class WorkoutPeriodSpecification : BaseSpecification<Workout>
{

    public WorkoutPeriodSpecification(Expression<Func<Workout, bool>> criteria) : base(criteria)
    {
        
    }
    
}