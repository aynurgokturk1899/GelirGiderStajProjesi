using IncomeIxpenseManager.DTOs.Transactions;

namespace IncomeIxpenseManager.Services;

public enum TransactionOperationStatus
{
    Success,
    CategoryNotFound,
    CategoryInactive,
    CategoryTypeMismatch
}

public sealed record TransactionOperationResult(
    TransactionOperationStatus Status,
    TransactionResponse? Transaction = null);
