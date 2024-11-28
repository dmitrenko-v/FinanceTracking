using Application.Dto.Goal;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;
    public GoalController(IGoalService goalService)
    {
        this._goalService = goalService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoals()
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return this.Ok(await this._goalService.GetUserGoalsAsync(userId!));
    }

    [HttpPost]
    public async Task<ActionResult> AddIncome(AddGoalDto addGoalDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._goalService.AddGoalAsync(addGoalDto, userId!);
        return this.Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGoal(int id, UpdateGoalDto updateGoalDto)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._goalService.UpdateGoalAsync(id, updateGoalDto, userId!);
        return this.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGoal(int id)
    {
        var userId = this.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        await this._goalService.DeleteGoalAsync(id, userId!);
        return this.Ok();
    }
}