namespace DataAccessLayer.Entities;

public class Card
{
    public string Number { get; set; }
    
    public string UserId { get; set; }
    
    public User User { get; set; }
}