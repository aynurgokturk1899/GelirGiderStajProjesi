using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Models;
using IncomeIxpenseManager.Validation;

namespace IncomeIxpenseManager.DTOs.Categories;

public sealed class CreateCategoryRequest
{
    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [NotWhiteSpace(ErrorMessage = "Kategori adı boşluklardan oluşamaz.")]
    [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
    public string Name { get; init; } = string.Empty;

    [EnumDataType(typeof(TransactionType), ErrorMessage = "Kategori türü Income (1) veya Expense (2) olmalıdır.")]
    public TransactionType Type { get; init; }
}
