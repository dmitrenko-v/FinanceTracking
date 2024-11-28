using Application.Dto.Auth;
using Application.Dto.Budget;
using Application.Dto.Expense;
using Application.Dto.Goal;
using Application.Dto.Income;
using Application.Identity;
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

        CreateMap<AddExpenseDto, Expense>();
        CreateMap<UpdateExpenseDto, Expense>();
        CreateMap<Expense, ExpenseDto>();

        CreateMap<RegisterDto, User>();
    }
}