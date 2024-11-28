using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        this._categoryService = categoryService;
    }

    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        return this.Ok(await this._categoryService.GetCategoriesAsync());
    }
}