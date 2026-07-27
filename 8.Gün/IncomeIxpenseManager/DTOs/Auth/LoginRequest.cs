using System.ComponentModel.DataAnnotations;

namespace IncomeIxpenseManager.DTOs.Auth;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola alanı zorunludur.")]
    public string Password { get; init; } = string.Empty;
}
