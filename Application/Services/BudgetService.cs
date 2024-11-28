using Application.Dto.Budget;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public BudgetService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }
    
    public async Task AddBudgetAsync(AddBudgetDto addBudgetDto, string userId)
    {
        var budget = this._mapper.Map<Budget>(addBudgetDto);

        if (await this._unitOfWork.CategoryRepository.FindByNameAsync(budget.CategoryName) is null)
        {
            throw new BadRequestException("There is no such category");
        }

        if (await this._unitOfWork.BudgetRepository.FindByUserIdAndCategoryName(userId, budget.CategoryName) is
            not null)
        {
            throw new BadRequestException("You already have budget in this category in this month");
        }
        
        budget.UserId = userId;
        // var userExpenses = await this._unitOfWork.ExpenseRepository.FindUserExpensesAsync(userId);
        budget.CurrentAmount = 0;
            // userExpenses
            // .Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year)
            // .Sum(x => x.Amount);
        
        this._unitOfWork.BudgetRepository.Add(budget);
        await this._unitOfWork.CommitAsync();
    }

    public async Task UpdateBudgetAsync(int id, decimal ceilingAmount, string userId)
    {
        Console.WriteLine(ceilingAmount);
        var budget = await this._unitOfWork.BudgetRepository.FindByIdAsync(id);

        ValidateBudget(budget, userId);
        
        budget!.CeilingAmount = ceilingAmount;
        budget.Id = id;
        this._unitOfWork.BudgetRepository.Update(budget);
        await this._unitOfWork.CommitAsync();
    }

    public async Task DeleteBudgetAsync(int id, string userId)
    {
        var budget = await this._unitOfWork.BudgetRepository.FindByIdAsync(id);

        ValidateBudget(budget, userId);
        
        this._unitOfWork.BudgetRepository.Delete(budget!);

        await this._unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<BudgetDto>> GetUserBudgetsAsync(string userId)
    {
        return this._mapper.Map<IEnumerable<BudgetDto>>(await this._unitOfWork.BudgetRepository.FindUserBudgetsAsync(userId));
    }

    private static void ValidateBudget(Budget? budget, string userId)
    {
        if (budget is null)
        {
            throw new NotFoundException("There is no budget with given id");
        }

        if (budget.UserId != userId)
        {
            throw new ForbiddenException();
        }
    }
}