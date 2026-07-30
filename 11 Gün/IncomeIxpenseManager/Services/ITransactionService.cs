using IncomeIxpenseManager.DTOs.Transactions;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.Services;

public interface ITransactionService
{
    Task<IReadOnlyList<TransactionResponse>> GetAllAsync(
        int userId,
        TransactionFilterRequest filter,
        CancellationToken cancellationToken);

    Task<TransactionResponse?> GetByIdAsync(
        int userId,
        int transactionId,
        CancellationToken cancellationToken);

    Task<TransactionOperationResult> CreateAsync(
        int userId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken);

    Task<TransactionOperationResult> UpdateAsync(
        int userId,
        int transactionId,
        UpdateTransactionRequest request,
        CancellationToken cancellationToken);

    Task<TransactionOperationStatus> DeleteAsync(
        int userId,
        int transactionId,
        CancellationToken cancellationToken);
}
