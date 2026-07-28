using IncomeIxpenseManager.DTOs.Transactions;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.Services;

public interface ITransactionService
{
    Task<IReadOnlyList<TransactionResponse>> GetAllAsync(
        int userId,
        TransactionType? type,
        int? categoryId,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken);

    Task<TransactionResponse?> GetByIdAsync(
        int userId,
        int transactionId,
        CancellationToken cancellationToken);

    Task<TransactionOperationResult> CreateAsync(
        int userId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken);
}
