using System.ComponentModel.DataAnnotations;

namespace IncomeIxpenseManager.DTOs.Auth;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir.")]
    public string LastName { get; init; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(256, ErrorMessage = "E-posta en fazla 256 karakter olabilir.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola alanı zorunludur.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Parola en az 8 karakter olmalıdır.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [Compare(nameof(Password), ErrorMessage = "Parolalar eşleşmiyor.")]
    public string ConfirmPassword { get; init; } = string.Empty;
}
