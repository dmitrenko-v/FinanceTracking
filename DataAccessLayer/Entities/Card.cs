using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class Card
{
    [Key]
    public string Number { get; set; }
    
    public string UserId { get; set; }
    
    public User User { get; set; }
}