namespace Application.Dto.Budget;

public class BudgetDto
{
    public int Id { get; set; }
    
    public decimal CurrentAmount { get; set; }
    
    public decimal CeilingAmount { get; set; }

    public string CategoryName { get; set; } = null!;
}