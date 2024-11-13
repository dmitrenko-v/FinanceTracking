namespace Domain.Entities;

public class Income
{
    public int Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string UserId { get; set; } = null!;
    
    public string CardNumber { get; set; } = null!;
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
    
    public Card Card { get; set; } = null!;
}