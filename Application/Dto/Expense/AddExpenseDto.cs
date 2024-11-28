using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Expense;

public class AddExpenseDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Title must be less than 100 characters long.")]
    public string Title { get; set; } = null!;
    
    [Required]
    [MaxLength(200, ErrorMessage = "Description must be less than 200 characters long.")]
    public string Description { get; set; } = null!;
    
    [Required]
    public string CategoryName { get; set; } = null!;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }
}