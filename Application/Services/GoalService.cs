using Application.Dto.Goal;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class GoalService : IGoalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }
    
    public async Task AddGoalAsync(AddGoalDto addGoalDto, string userId)
    {
        var goal = this._mapper.Map<Goal>(addGoalDto);
        
        goal.UserId = userId;
        
        this._unitOfWork.GoalRepository.Add(goal);
        await this._unitOfWork.CommitAsync();
    }

    public async Task UpdateGoalAsync(int id, UpdateGoalDto updateGoalDto, string userId)
    {
        var goal = await this._unitOfWork.GoalRepository.FindByIdAsync(id);

        ValidateGoal(goal, userId);
        
        var updatedGoal = this._mapper.Map<Goal>(updateGoalDto);
        updatedGoal.Id = id;
        updatedGoal.UserId = userId;
        
        this._unitOfWork.GoalRepository.Update(updatedGoal);
        await this._unitOfWork.CommitAsync();
    }

    public async Task DeleteGoalAsync(int id, string userId)
    {
        var goal = await this._unitOfWork.GoalRepository.FindByIdAsync(id);
        
        ValidateGoal(goal, userId);

        this._unitOfWork.GoalRepository.Delete(goal!);
        await this._unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<GoalDto>> GetUserGoalsAsync(string userId)
    {
        return this._mapper.Map<IEnumerable<GoalDto>>(await this._unitOfWork.GoalRepository.FindUserGoalsAsync(userId));
    }

    private static void ValidateGoal(Goal? goal, string userId)
    {
        if (goal is null)
        {
            throw new NotFoundException("There is no goal with given id");
        }
        
        if (goal.UserId != userId)
        {
            throw new ForbiddenException();
        }
    }
}