using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public class Income
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string UserId { get; set; }
    
    [ForeignKey("Card")]
    public string CardNumber { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
    
    public User User { get; set; }
    
    public Card Card { get; set; }
}