using IncomeIxpenseManager.DTOs.Categories;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.Services;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryResponse>> GetAllAsync(
        int userId,
        TransactionType? type,
        bool? isActive,
        CancellationToken cancellationToken);

    Task<CategoryResponse?> CreateAsync(
        int userId,
        CreateCategoryRequest request,
        CancellationToken cancellationToken);

    Task<CategoryOperationResult> UpdateAsync(
        int userId,
        int categoryId,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken);

    Task<CategoryOperationStatus> DeleteAsync(
        int userId,
        int categoryId,
        CancellationToken cancellationToken);
}
