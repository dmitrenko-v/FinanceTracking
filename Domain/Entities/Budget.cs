namespace Domain.Entities;

public class Budget
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    
    public decimal CurrentAmount { get; set; }
    
    public decimal CeilingAmount { get; set; }

    public string CategoryName { get; set; } = null!;

    public Category Category { get; set; } = null!;
}