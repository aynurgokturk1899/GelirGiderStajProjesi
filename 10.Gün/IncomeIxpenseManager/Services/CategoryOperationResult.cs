using IncomeIxpenseManager.DTOs.Categories;

namespace IncomeIxpenseManager.Services;

public enum CategoryOperationStatus
{
    Success,
    NotFound,
    Duplicate,
    TypeInUse
}

public sealed record CategoryOperationResult(
    CategoryOperationStatus Status,
    CategoryResponse? Category = null);
