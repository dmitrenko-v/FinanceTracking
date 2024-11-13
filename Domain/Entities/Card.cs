namespace Domain.Entities;

public class Card
{
    public string Number { get; set; } = null!;

    public string UserId { get; set; } = null!;
    
    public IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();

    public IEnumerable<Income> Incomes { get; set; } = new List<Income>();
}