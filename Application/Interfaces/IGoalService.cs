using Application.Dto.Goal;

namespace Application.Interfaces;

public interface IGoalService
{
    public Task AddGoalAsync(AddGoalDto addGoalDto, string userId);
    
    public Task UpdateGoalAsync(int id, UpdateGoalDto updateGoalDto, string userId);
    
    public Task DeleteGoalAsync(int id, string userId);
    
    public Task<IEnumerable<GoalDto>> GetUserGoalsAsync(string userId);
}