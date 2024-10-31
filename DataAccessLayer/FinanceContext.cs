using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public class FinanceContext : IdentityDbContext<User>
{
    public FinanceContext(DbContextOptions<FinanceContext> options)
        : base(options)
    {
    }

    public DbSet<AccountType> AccountTypes { get; set; }

    public DbSet<Budget> Budgets { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Card> Cards { get; set; }

    public DbSet<Expense> Expenses { get; set; }

    public DbSet<Income> Incomes { get; set; }

    public DbSet<Goal> Goals { get; set; }
}