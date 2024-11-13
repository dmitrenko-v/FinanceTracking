namespace Domain.Entities;

public class Goal
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public string UserId { get; set; } = null!;

    public decimal GoalAmount { get; set; }

    public decimal CurrentAmount { get; set; }
   
    public string StoredIn { get; set; } = null!;

    public DateTime Deadline { get; set; }
}