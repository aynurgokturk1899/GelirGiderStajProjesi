using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.DTOs.Transactions;

public sealed record TransactionResponse(
    int Id,
    int CategoryId,
    string CategoryName,
    TransactionType Type,
    decimal Amount,
    DateOnly TransactionDate,
    string? Description,
    DateTime CreatedDate);
