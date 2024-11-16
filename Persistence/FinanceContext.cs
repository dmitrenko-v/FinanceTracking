using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;

namespace Persistence;

public class FinanceContext : IdentityDbContext<User>
{
    public FinanceContext(DbContextOptions<FinanceContext> options)
        : base(options)
    {
    }

    public DbSet<AccountType> AccountTypes { get; set; }

    public DbSet<Budget> Budgets { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Expense> Expenses { get; set; }

    public DbSet<Income> Incomes { get; set; }

    public DbSet<Goal> Goals { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AccountType>().HasKey(x => x.Type);
        builder.Entity<AccountType>().HasMany<User>().WithOne(x => x.AccountType).HasForeignKey("AccountTypeName");

        builder.Entity<Category>().HasKey(x => x.Name);

        builder.Entity<Budget>().HasOne<User>().WithMany(x => x.Budgets).HasForeignKey(x => x.UserId);
        builder.Entity<Budget>().HasOne(x => x.Category).WithMany(x => x.Budgets).HasForeignKey(x => x.CategoryName);

        builder.Entity<Expense>().HasOne<User>().WithMany(x => x.Expenses).HasForeignKey(x => x.UserId);
        builder.Entity<Expense>().HasOne(x => x.Category).WithMany(x => x.Expenses).HasForeignKey(x => x.CategoryName);

        builder.Entity<Income>().HasOne<User>().WithMany(x => x.Incomes).HasForeignKey(x => x.UserId);

        builder.Entity<Goal>().HasOne<User>().WithMany(x => x.Goals).HasForeignKey(x => x.UserId);
    }
}