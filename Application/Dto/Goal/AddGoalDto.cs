using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Goal;

public class AddGoalDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Title must be less than 100 characters long.")]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(100, ErrorMessage = "Description must be less than 100 characters long.")]
    public string Description { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal GoalAmount { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CurrentAmount { get; set; }
   
    [Required]
    [MaxLength(50, ErrorMessage = "Storage type must be less than 50 characters long.")]
    public string StoredIn { get; set; } = null!;

    [Required]
    public DateTime Deadline { get; set; }
}