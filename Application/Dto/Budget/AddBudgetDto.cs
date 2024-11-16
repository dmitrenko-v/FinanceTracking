using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Budget;
public class AddBudgetDto
{
    [Required]
    [Range(1, double.MaxValue)]
    public decimal CeilingAmount { get; set; }

    [Required]
    public string CategoryName { get; set; } = null!;
    
    [Required]
    public DateTime EndDate { get; set; }
}