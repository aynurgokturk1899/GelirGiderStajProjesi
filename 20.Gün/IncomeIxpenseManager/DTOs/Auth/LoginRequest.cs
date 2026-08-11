using System.ComponentModel.DataAnnotations;
using IncomeIxpenseManager.Validation;

namespace IncomeIxpenseManager.DTOs.Auth;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [NotWhiteSpace(ErrorMessage = "E-posta alanı boşluklardan oluşamaz.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(256, ErrorMessage = "E-posta en fazla 256 karakter olabilir.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Parola alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Parola en fazla 100 karakter olabilir.")]
    public string Password { get; init; } = string.Empty;
}
