using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly DbSet<Goal> _goals;

    public GoalRepository(FinanceContext context)
    {
        this._goals = context.Goals;
    }
    
    public void Add(Goal entity)
    {
        this._goals.Add(entity);
    }

    public void Delete(Goal entity)
    {
        this._goals.Remove(entity);
    }

    public void Update(Goal entity)
    {
        this._goals.Update(entity);
    }
}