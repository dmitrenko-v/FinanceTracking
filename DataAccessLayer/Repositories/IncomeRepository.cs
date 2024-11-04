using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly DbSet<Income> _incomes;
    
    public IncomeRepository(FinanceContext context)
    {
        this._incomes = context.Incomes;
    }
    
    public void Add(Income entity)
    {
        this._incomes.Add(entity);
    }

    public void Delete(Income entity)
    {
        this._incomes.Remove(entity);
    }

    public void Update(Income entity)
    {
        this._incomes.Update(entity);
    }
}