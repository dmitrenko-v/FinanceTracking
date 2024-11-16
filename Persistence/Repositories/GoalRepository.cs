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

    public async Task<Goal?> FindByIdAsync(int id)
    {
        return await this._goals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Goal>> FindUserGoalsAsync(string userId)
    {
        return await this._goals.Where(x => x.UserId == userId).ToListAsync();
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