using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbSet<Category> _categories;

    public CategoryRepository(FinanceContext context)
    {
        this._categories = context.Categories;    
    }
    
    public void Add(Category entity)
    {
        this._categories.Add(entity);
    }

    public void Delete(Category entity)
    {
        this._categories.Remove(entity);
    }

    public void Update(Category entity)
    {
        this._categories.Update(entity);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await this._categories.ToListAsync();
    }
}