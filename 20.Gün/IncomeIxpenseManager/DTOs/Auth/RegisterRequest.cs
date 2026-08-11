using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Validation;

namespace IncomeIxpenseManager.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [NotWhiteSpace(ErrorMessage = "Ad alanı boşluklardan oluşamaz.")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [NotWhiteSpace(ErrorMessage = "Soyad alanı boşluklardan oluşamaz.")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir.")]
    public string LastName { get; init; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [NotWhiteSpace(ErrorMessage = "E-posta alanı boşluklardan oluşamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(256, ErrorMessage = "E-posta en fazla 256 karakter olabilir.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola alanı zorunludur.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola 8 ile 100 karakter arasında olmalıdır.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [Compare(nameof(Password), ErrorMessage = "Parolalar eşleşmiyor.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
