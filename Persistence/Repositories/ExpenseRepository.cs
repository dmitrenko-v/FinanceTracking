using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DbSet<Expense> _expenses;

    public ExpenseRepository(FinanceContext context)
    {
        this._expenses = context.Expenses;
    }

    public async Task<Expense?> FindByIdAsync(int id)
    {
        return await this._expenses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);;
    }

    public async Task<IEnumerable<Expense>> FindUserExpensesAsync(string userId)
    {
        return await this._expenses.Where(x => x.UserId == userId).ToListAsync();
    }

    public void Add(Expense entity)
    {
        this._expenses.Add(entity);
    }

    public void Delete(Expense entity)
    {
        this._expenses.Remove(entity);
    }

    public void Update(Expense entity)
    {
        this._expenses.Update(entity);
    }
}