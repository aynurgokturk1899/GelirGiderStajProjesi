using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.DTOs.Categories;

public sealed record CategoryResponse(
    int Id,
    string Name,
    TransactionType Type,
    bool IsActive);
