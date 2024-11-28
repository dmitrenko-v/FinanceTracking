using Application.Dto.Income;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncomeController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    
    public IncomeController(IIncomeService incomeService)
    {
        this._incomeService = incomeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncomeDto>>> GetIncomes()
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return this.Ok(await this._incomeService.GetUserIncomesAsync(userId!));
    }

    [HttpPost]
    public async Task<ActionResult> AddIncome(AddIncomeDto addIncomeDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._incomeService.AddIncomeAsync(addIncomeDto, userId!);
        return this.Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateIncome(int id, UpdateIncomeDto updateIncomeDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._incomeService.UpdateIncomeAsync(id, updateIncomeDto, userId!);
        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteIncome(int id)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._incomeService.DeleteIncomeAsync(id, userId!);
        return this.Ok();
    }
}