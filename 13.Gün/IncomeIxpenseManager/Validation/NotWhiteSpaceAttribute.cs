using System.ComponentModel.DataAnnotations;

namespace IncomeIxpenseManager.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class NotWhiteSpaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is not string text
               || text.Length == 0
               || !string.IsNullOrWhiteSpace(text);
    }
}
