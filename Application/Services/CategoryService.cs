using Application.Interfaces;
using Domain.Repositories;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<string>> GetCategoriesAsync()
    {
        return (await this._unitOfWork.CategoryRepository.GetAllCategoriesAsync()).Select(category => category.Name);
    }
}