using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly DbSet<Budget> _budgets;

    public BudgetRepository(FinanceContext context)
    {
        this._budgets = context.Budgets;
    }

    public async Task<Budget?> FindByIdAsync(int id)
    {
        return await this._budgets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Budget?> FindByUserIdAndCategoryName(string userId, string categoryName)
    {
        return await this._budgets.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.CategoryName == categoryName);
    }

    public async Task<IEnumerable<Budget>> FindUserBudgetsAsync(string userId)
    {
        return await this._budgets.Where(x => x.UserId == userId).ToListAsync();
    }

    public void Add(Budget entity)
    {
        this._budgets.Add(entity);
    }

    public void Delete(Budget entity)
    {
        this._budgets.Remove(entity);
    }

    public void Update(Budget entity)
    {
        this._budgets.Update(entity);
    }
}