namespace DataAccessLayer.Entities;

public class Goal
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
    
    public string UserId { get; set; }

    public decimal GoalAmount { get; set; }

    public decimal CurrentAmount { get; set; }

    public string StoredIn { get; set; }

    public DateTime Deadline { get; set; }
    
    public User User { get; set; }
}