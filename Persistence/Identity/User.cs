using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
    
    public AccountType AccountType { get; set; } = null!;
    
    public IEnumerable<Budget> Budgets { get; set; } = new List<Budget>();
    
    public IEnumerable<Card> Cards { get; set; } = new List<Card>();
    
    public IEnumerable<Income> Incomes { get; set; } = new List<Income>();
    
    public IEnumerable<Goal> Goals { get; set; } = new List<Goal>();
    
    public IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();
}