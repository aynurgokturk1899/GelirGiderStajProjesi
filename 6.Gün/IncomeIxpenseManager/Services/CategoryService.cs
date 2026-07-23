using IncomeIxpenseManager.Data;
using IncomeIxpenseManager.DTOs.Categories;
using IncomeIxpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Services;

public sealed class CategoryService(ApplicationDbContext dbContext) : ICategoryService
{
    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(
        int userId,
        TransactionType? type,
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Categories
            .AsNoTracking()
            .Where(category => category.UserId == userId);

        if (!includeInactive)
        {
            query = query.Where(category => category.IsActive);
        }

        if (type.HasValue)
        {
            query = query.Where(category => category.Type == type.Value);
        }

        return await query
            .OrderBy(category => category.Type)
            .ThenBy(category => category.Name)
            .Select(category => new CategoryResponse(
                category.Id,
                category.Name,
                category.Type,
                category.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryResponse?> CreateAsync(
        int userId,
        CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return null;
        }

        var alreadyExists = await dbContext.Categories.AnyAsync(
            category => category.UserId == userId
                        && category.Name == normalizedName
                        && category.Type == request.Type,
            cancellationToken);

        if (alreadyExists)
        {
            return null;
        }

        var category = new Category
        {
            UserId = userId,
            Name = normalizedName,
            Type = request.Type
        };

        dbContext.Categories.Add(category);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return null;
        }

        return new CategoryResponse(category.Id, category.Name, category.Type, category.IsActive);
    }
}
