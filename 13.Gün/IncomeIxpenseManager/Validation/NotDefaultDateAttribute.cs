using System.ComponentModel.DataAnnotations;

namespace IncomeIxpenseManager.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class NotDefaultDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is DateOnly date && date != default;
    }
}
