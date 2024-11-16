using Domain.Entities;

namespace Domain.Repositories;

public interface IBudgetRepository : IRepository<Budget>
{
    public Task<Budget?> FindByIdAsync(int id);
    
    public Task<Budget?> FindByUserIdAndCategoryName(string userId, string categoryName);
    
    public Task<IEnumerable<Budget>> FindUserBudgetsAsync(string userId);
}