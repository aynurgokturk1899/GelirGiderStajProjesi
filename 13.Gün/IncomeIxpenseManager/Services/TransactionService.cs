using IncomeIxpenseManager.Data;
using IncomeIxpenseManager.DTOs.Transactions;
using IncomeIxpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Services;

public sealed class TransactionService(ApplicationDbContext dbContext) : ITransactionService
{
    public async Task<IReadOnlyList<TransactionResponse>> GetAllAsync(
        int userId,
        TransactionFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.UserId == userId);

        if (filter.Type.HasValue)
        {
            query = query.Where(transaction => transaction.Type == filter.Type.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(transaction => transaction.CategoryId == filter.CategoryId.Value);
        }

        if (filter.CategoryType.HasValue)
        {
            query = query.Where(transaction =>
                transaction.Category.Type == filter.CategoryType.Value);
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.TransactionDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(transaction =>
                transaction.TransactionDate <= filter.EndDate.Value);
        }

        return await query
            .OrderByDescending(transaction => transaction.TransactionDate)
            .ThenByDescending(transaction => transaction.CreatedDate)
            .ThenByDescending(transaction => transaction.Id)
            .Select(transaction => new TransactionResponse(
                transaction.Id,
                transaction.CategoryId,
                transaction.Category.Name,
                transaction.Type,
                transaction.Amount,
                transaction.TransactionDate,
                transaction.Description,
                transaction.CreatedDate))
            .ToListAsync(cancellationToken);
    }

    public Task<TransactionResponse?> GetByIdAsync(
        int userId,
        int transactionId,
        CancellationToken cancellationToken)
    {
        return dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.Id == transactionId && transaction.UserId == userId)
            .Select(transaction => new TransactionResponse(
                transaction.Id,
                transaction.CategoryId,
                transaction.Category.Name,
                transaction.Type,
                transaction.Amount,
                transaction.TransactionDate,
                transaction.Description,
                transaction.CreatedDate))
            .SingleOrDefaultAsync(cancellationToken);
    }

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

    public async Task<TransactionOperationResult> UpdateAsync(
        int userId,
        int transactionId,
        UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Transactions.SingleOrDefaultAsync(
            item => item.Id == transactionId && item.UserId == userId,
            cancellationToken);

        if (transaction is null)
        {
            return new TransactionOperationResult(TransactionOperationStatus.NotFound);
        }

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

        transaction.CategoryId = category.Id;
        transaction.Type = request.Type;
        transaction.Amount = request.Amount;
        transaction.TransactionDate = request.TransactionDate;
        transaction.Description = string.IsNullOrWhiteSpace(request.Description)
            ? null
            : request.Description.Trim();

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

    public async Task<TransactionOperationStatus> DeleteAsync(
        int userId,
        int transactionId,
        CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Transactions.SingleOrDefaultAsync(
            item => item.Id == transactionId && item.UserId == userId,
            cancellationToken);

        if (transaction is null)
        {
            return TransactionOperationStatus.NotFound;
        }

        dbContext.Transactions.Remove(transaction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return TransactionOperationStatus.Success;
    }
}
