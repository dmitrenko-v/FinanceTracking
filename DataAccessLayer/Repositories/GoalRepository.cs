using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

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