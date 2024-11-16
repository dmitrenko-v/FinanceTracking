using Application.Dto.Expense;
using Application.Dto.Income;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task AddExpenseAsync(AddExpenseDto addExpenseDto, string userId)
    {
        var expense = this._mapper.Map<Expense>(addExpenseDto);

        if (await this._unitOfWork.CategoryRepository.FindByNameAsync(expense.CategoryName) is null)
        {
            throw new BadRequestException("There is no such category");
        }

        expense.UserId = userId;

        this._unitOfWork.ExpenseRepository.Add(expense);
        await this._unitOfWork.CommitAsync();
    }

    public async Task UpdateExpenseAsync(int id, UpdateExpenseDto updateExpenseDto, string userId)
    {
        var expense = await this._unitOfWork.ExpenseRepository.FindByIdAsync(id);

        // ValidateGoal(expense, userId);

        var updatedExpense = this._mapper.Map<Expense>(updateExpenseDto);
        updatedExpense.Id = id;
        updatedExpense.UserId = userId;
        updatedExpense.Date = expense!.Date;

        this._unitOfWork.ExpenseRepository.Update(updatedExpense);
        await this._unitOfWork.CommitAsync();
    }

    public async Task DeleteExpenseAsync(int id, string userId)
    {
        var expense = await this._unitOfWork.ExpenseRepository.FindByIdAsync(id);

        ValidateExpense(expense, userId);

        this._unitOfWork.ExpenseRepository.Delete(expense!);
        await this._unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<ExpenseDto>> GetUserExpensesAsync(string userId)
    {
        return this._mapper.Map<IEnumerable<ExpenseDto>>(await this._unitOfWork.ExpenseRepository.FindUserExpensesAsync(userId));
    }

    private static void ValidateExpense(Expense? expense, string userId)
    {
        if (expense is null)
        {
            throw new NotFoundException("There is no expense with given id");
        }

        if (expense.UserId != userId)
        {
            throw new ForbiddenException();
        }
    }
}