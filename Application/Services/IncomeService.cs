using Application.Dto.Income;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Application.Services;

public class IncomeService : IIncomeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IncomeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task AddIncomeAsync(AddIncomeDto addIncomeDto, string userId)
    {
        var income = this._mapper.Map<Income>(addIncomeDto);

        income.UserId = userId;

        this._unitOfWork.IncomeRepository.Add(income);
        await this._unitOfWork.CommitAsync();
    }

    public async Task UpdateIncomeAsync(int id, UpdateIncomeDto updateIncomeDto, string userId)
    {
        var income = await this._unitOfWork.IncomeRepository.FindByIdAsync(id);

        ValidateIncome(income, userId);
        
        income!.Date = updateIncomeDto.Date;
        income.Amount = updateIncomeDto.Amount;
        income.Description = updateIncomeDto.Description;
        income.Title = updateIncomeDto.Title;

        this._unitOfWork.IncomeRepository.Update(income);
        await this._unitOfWork.CommitAsync();
    }

    public async Task DeleteIncomeAsync(int id, string userId)
    {
        var income = await this._unitOfWork.IncomeRepository.FindByIdAsync(id);
        
        ValidateIncome(income, userId);

        this._unitOfWork.IncomeRepository.Delete(income!);
        await this._unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<IncomeDto>> GetUserIncomesAsync(string userId)
    {
        return this._mapper.Map<IEnumerable<IncomeDto>>(await this._unitOfWork.IncomeRepository.FindUserIncomesAsync(userId));
    }

    private static void ValidateIncome(Income? income, string userId)
    {
        if (income is null)
        {
            throw new NotFoundException("There is no income with given id");
        }
        
        if (income.UserId != userId)
        {
            throw new ForbiddenException();
        }
    }
}