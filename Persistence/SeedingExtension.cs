using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public static class SeedingExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "User", NormalizedName = "USER" },
            new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" });

        modelBuilder.Entity<AccountType>().HasData(
            new AccountType { Type = "Base" }, new AccountType { Type = "Premium" });

        modelBuilder.Entity<Category>().HasData(
            new Category { Name = "Food" },
            new Category { Name = "Entertainment" },
            new Category { Name = "Healthcare" },
            new Category { Name = "Travel" },
            new Category { Name = "Shopping" });
    }
}