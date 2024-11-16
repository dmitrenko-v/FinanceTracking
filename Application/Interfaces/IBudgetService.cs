using Application.Dto.Budget;

namespace Application.Interfaces;

public interface IBudgetService
{
    public Task AddBudgetAsync(AddBudgetDto addBudgetDto, string userId);

    public Task UpdateBudgetAsync(int id, decimal ceilingAmount, string userId);
    
    public Task DeleteBudgetAsync(int id, string userId);
    
    public Task<IEnumerable<BudgetDto>> GetUserBudgetsAsync(string userId);
}