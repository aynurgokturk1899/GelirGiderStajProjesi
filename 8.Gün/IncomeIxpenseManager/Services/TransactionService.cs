using IncomeIxpenseManager.Data;
using IncomeIxpenseManager.DTOs.Transactions;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Services;

public sealed class TransactionService(ApplicationDbContext dbContext) : ITransactionService
{
    public async Task<TransactionOperationResult> CreateAsync(
        int userId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(
                item => item.Id == request.CategoryId && item.UserId == userId,
                cancellationToken);

        if (category is null)
        {
            return new TransactionOperationResult(TransactionOperationStatus.CategoryNotFound);
        }

        if (!category.IsActive)
        {
            return new TransactionOperationResult(TransactionOperationStatus.CategoryInactive);
        }

        if (category.Type != request.Type)
        {
            return new TransactionOperationResult(TransactionOperationStatus.CategoryTypeMismatch);
        }

        var description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();

        var transaction = new Models.Transaction
        {
            UserId = userId,
            CategoryId = category.Id,
            Type = request.Type,
            Amount = request.Amount,
            TransactionDate = request.TransactionDate,
            Description = description
        };

        dbContext.Transactions.Add(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new TransactionOperationResult(
            TransactionOperationStatus.Success,
            new TransactionResponse(
                transaction.Id,
                transaction.CategoryId,
                category.Name,
                transaction.Type,
                transaction.Amount,
                transaction.TransactionDate,
                transaction.Description,
                transaction.CreatedDate));
    }
}
