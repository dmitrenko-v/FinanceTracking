using Domain.Entities;

namespace Domain.Repositories;

public interface IGoalRepository : IRepository<Goal>
{
    public Task<Goal?> FindByIdAsync(int id);
    
    public Task<IEnumerable<Goal>> FindUserGoalsAsync(string userId);
}