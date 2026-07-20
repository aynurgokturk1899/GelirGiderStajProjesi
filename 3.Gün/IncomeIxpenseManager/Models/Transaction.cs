namespace IncomeIxpenseManager.Models;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
