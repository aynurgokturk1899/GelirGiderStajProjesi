using System.ComponentModel.DataAnnotations;

namespace IncomeIxpenseManager.DTOs.Dashboard;

public sealed class DashboardSummaryFilterRequest : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Kategori kimliği sıfırdan büyük olmalıdır.")]
    public int? CategoryId { get; init; }

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
    }
}
