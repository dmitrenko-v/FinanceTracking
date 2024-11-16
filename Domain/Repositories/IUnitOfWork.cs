namespace Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    public Task CommitAsync();
    
    public IBudgetRepository BudgetRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IExpenseRepository ExpenseRepository { get; }
    public IIncomeRepository IncomeRepository { get; }
    public IGoalRepository GoalRepository { get; }
}