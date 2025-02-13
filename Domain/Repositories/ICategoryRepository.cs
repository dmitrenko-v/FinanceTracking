using Domain.Entities;

namespace Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync();

    public Task<Category?> FindByNameAsync(string name);
}