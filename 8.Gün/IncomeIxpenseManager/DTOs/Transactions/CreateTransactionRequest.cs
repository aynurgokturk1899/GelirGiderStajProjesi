using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.DTOs.Transactions;

public sealed class CreateTransactionRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçilmelidir.")]
    public int CategoryId { get; init; }

    [EnumDataType(typeof(TransactionType), ErrorMessage = "İşlem türü Income (1) veya Expense (2) olmalıdır.")]
    public TransactionType Type { get; init; }

    public decimal Amount { get; init; }

    public DateOnly TransactionDate { get; init; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; init; }
}
