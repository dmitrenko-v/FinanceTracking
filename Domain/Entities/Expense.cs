namespace Domain.Entities;

public class Expense
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
    
    public string CategoryName { get; set; } = null!;
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
    
    public Category Category { get; set; } = null!;
}