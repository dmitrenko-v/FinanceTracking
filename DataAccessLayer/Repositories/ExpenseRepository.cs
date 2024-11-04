using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DbSet<Expense> _expenses;

    public ExpenseRepository(FinanceContext context)
    {
        this._expenses = context.Expenses;
    }
    
    public void Add(Expense entity)
    {
        this._expenses.Add(entity);
    }

    public void Delete(Expense entity)
    {
        this._expenses.Remove(entity);
    }

    public void Update(Expense entity)
    {
        this._expenses.Update(entity);
    }
}