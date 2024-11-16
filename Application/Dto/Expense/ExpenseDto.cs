namespace Application.Dto.Expense;

public class ExpenseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string CategoryName { get; set; } = null!;
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
}