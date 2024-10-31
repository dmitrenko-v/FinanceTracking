using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public class Budget
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public decimal CurrentAmount { get; set; }
    
    public decimal CeilingAmount { get; set; }
    
    [ForeignKey("Category")]
    public string CategoryName { get; set; }
    
    public Category Category { get; set; }
    
    public User User { get; set; }
}