using Domain.Repositories;

namespace Application.Interfaces;

public interface ICategoryService
{
   public Task<IEnumerable<string>> GetCategoriesAsync();
}