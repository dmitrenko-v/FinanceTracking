using Application.Dto.Expense;
using Application.Dto.Income;

namespace Application.Interfaces;

public interface IExpenseService
{
    public Task AddExpenseAsync(AddExpenseDto addExpenseDto, string userId);

    public Task UpdateExpenseAsync(int id, UpdateExpenseDto updateExpenseDto, string userId);
    
    public Task DeleteExpenseAsync(int id, string userId);
    
    public Task<IEnumerable<ExpenseDto>> GetUserExpensesAsync(string userId);
}