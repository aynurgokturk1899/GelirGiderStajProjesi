using IncomeIxpenseManager.DTOs.Transactions;

namespace IncomeIxpenseManager.Services;

public interface ITransactionService
{
    Task<TransactionOperationResult> CreateAsync(
        int userId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken);
}
