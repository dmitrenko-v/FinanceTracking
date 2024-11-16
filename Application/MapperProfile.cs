using Application.Dto.Budget;
using Application.Dto.Expense;
using Application.Dto.Goal;
using Application.Dto.Income;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AddBudgetDto, Budget>();
        CreateMap<Budget, BudgetDto>();
        
        CreateMap<AddGoalDto, Goal>();
        CreateMap<UpdateGoalDto, Goal>();
        CreateMap<Goal, GoalDto>();
        
        CreateMap<AddIncomeDto, Income>();
        CreateMap<UpdateIncomeDto, Income>();
        CreateMap<Income, IncomeDto>();

        CreateMap<AddExpenseDto, ExpenseDto>();
        CreateMap<UpdateExpenseDto, ExpenseDto>();
        CreateMap<Expense, ExpenseDto>();
    }
}