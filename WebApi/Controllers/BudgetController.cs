using Application.Dto.Budget;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    
    public BudgetController(IBudgetService budgetService)
    {
        this._budgetService = budgetService;    
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets()
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return this.Ok(await this._budgetService.GetUserBudgetsAsync(userId!));
    }

    [HttpPost]
    public async Task<ActionResult> AddBudget(AddBudgetDto addBudgetDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._budgetService.AddBudgetAsync(addBudgetDto, userId!);
        return this.Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBudget(int id, decimal ceilingAmount)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._budgetService.UpdateBudgetAsync(id, ceilingAmount, userId!);
        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBudget(int id)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._budgetService.DeleteBudgetAsync(id, userId!);
        return this.Ok();
    }
}