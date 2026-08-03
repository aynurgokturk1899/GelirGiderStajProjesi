using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.DTOs.Transactions;

public sealed class TransactionFilterRequest : IValidatableObject
{
    [EnumDataType(
        typeof(TransactionType),
        ErrorMessage = "İşlem türü Income (1) veya Expense (2) olmalıdır.")]
    public TransactionType? Type { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Kategori kimliği sıfırdan büyük olmalıdır.")]
    public int? CategoryId { get; init; }

    [EnumDataType(
        typeof(TransactionType),
        ErrorMessage = "Kategori türü Income (1) veya Expense (2) olmalıdır.")]
    public TransactionType? CategoryType { get; init; }

    public DateOnly? StartDate { get; init; }

    public DateOnly? EndDate { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            yield return new ValidationResult(
                "Başlangıç tarihi bitiş tarihinden sonra olamaz.",
                [nameof(StartDate)]);
        }

        if (Type.HasValue && CategoryType.HasValue && Type != CategoryType)
        {
            yield return new ValidationResult(
                "İşlem türü ile kategori türü birbiriyle uyumlu olmalıdır.",
                [nameof(Type), nameof(CategoryType)]);
        }
    }
}
