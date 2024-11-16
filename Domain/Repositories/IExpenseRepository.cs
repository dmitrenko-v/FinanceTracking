using Domain.Entities;

namespace Domain.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    public Task<Expense?> FindByIdAsync(int id);

    public Task<IEnumerable<Expense>> FindUserExpensesAsync(string userId);
}