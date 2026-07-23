using IncomeIxpenseManager.DTOs.Categories;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponse>> GetAllAsync(
        int userId,
        TransactionType? type,
        bool includeInactive,
        CancellationToken cancellationToken);

    Task<CategoryResponse?> CreateAsync(
        int userId,
        CreateCategoryRequest request,
        CancellationToken cancellationToken);
}
