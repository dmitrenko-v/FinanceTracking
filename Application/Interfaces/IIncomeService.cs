using Application.Dto.Income;

namespace Application.Interfaces;

public interface IIncomeService
{
    public Task AddIncomeAsync(AddIncomeDto addIncomeDto, string userId);
    
    public Task UpdateIncomeAsync(int id, UpdateIncomeDto updateIncomeDto, string userId);
    
    public Task DeleteIncomeAsync(int id, string userId);
    
    public Task<IEnumerable<IncomeDto>> GetUserIncomesDto(string userId);
}