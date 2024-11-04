using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly DbSet<Budget> _budgets;

    public BudgetRepository(FinanceContext context)
    {
        this._budgets = context.Budgets;
    }
    
    public void Add(Budget entity)
    {
        this._budgets.Add(entity);
    }

    public void Delete(Budget entity)
    {
        this._budgets.Remove(entity);
    }

    public void Update(Budget entity)
    {
        this._budgets.Update(entity);
    }
}