using Application.Dto.Expense;
using Application.Dto.Goal;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    public ExpenseController(IExpenseService expenseService)
    {
        this._expenseService = expenseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoalsAsync()
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return this.Ok(await this._expenseService.GetUserExpensesAsync(userId!));
    }
    

    [HttpPost]
    public async Task<ActionResult> AddExpense(AddExpenseDto addExpenseDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._expenseService.AddExpenseAsync(addExpenseDto, userId!);
        return this.Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._expenseService.UpdateExpenseAsync(id, updateExpenseDto, userId!);
        return this.Ok();
    }

    [HttpDelete("{id}")] public async Task<ActionResult> DeleteExpense(int id)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._expenseService.DeleteExpenseAsync(id, userId!);
        return this.Ok();
    }
}