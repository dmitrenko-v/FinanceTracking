using Persistence.Repositories;
using Domain.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly FinanceContext _context;

    public IBudgetRepository BudgetRepository => new BudgetRepository(_context);
    public ICategoryRepository CategoryRepository => new CategoryRepository(_context);
    public IExpenseRepository ExpenseRepository => new ExpenseRepository(_context);
    public IIncomeRepository IncomeRepository => new IncomeRepository(_context);
    public IGoalRepository GoalRepository => new GoalRepository(_context);

    public UnitOfWork(FinanceContext context)
    {
        this._context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}