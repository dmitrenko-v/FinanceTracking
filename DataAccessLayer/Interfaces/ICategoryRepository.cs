using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync();
}