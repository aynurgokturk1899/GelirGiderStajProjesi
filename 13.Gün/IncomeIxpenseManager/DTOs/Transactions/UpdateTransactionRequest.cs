using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Models;
using IncomeIxpenseManager.Validation;

namespace IncomeIxpenseManager.DTOs.Transactions;

public sealed class UpdateTransactionRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçilmelidir.")]
    public int CategoryId { get; init; }

    [EnumDataType(typeof(TransactionType), ErrorMessage = "İşlem türü Income (1) veya Expense (2) olmalıdır.")]
    public TransactionType Type { get; init; }

    [Range(
        typeof(decimal),
        "0.01",
        "9999999999999999.99",
        ErrorMessage = "Tutar 0,01 ile 9.999.999.999.999.999,99 arasında olmalıdır.")]
    public decimal Amount { get; init; }

    [NotDefaultDate(ErrorMessage = "İşlem tarihi zorunludur.")]
    public DateOnly TransactionDate { get; init; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; init; }
}
