using Domain.Entities;

namespace Domain.Repositories;

public interface IIncomeRepository : IRepository<Income>
{
    public Task<Income?> FindByIdAsync(int id);
    
    public Task<IEnumerable<Income>> FindUserIncomesAsync(string userId);
}