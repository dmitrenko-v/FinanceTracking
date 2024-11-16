namespace Domain.Entities;

public class Category
{
    public string Name { get; set; } = null!;
    
    public string? AuthorId { get; set; }
    
    public IEnumerable<Budget> Budgets { get; set; } = new List<Budget>();
    
    public IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();
}