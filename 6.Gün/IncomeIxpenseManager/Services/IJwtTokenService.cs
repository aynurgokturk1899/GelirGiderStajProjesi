using IncomeIxpenseManager.DTOs.Auth;
using IncomeIxpenseManager.Models;

namespace IncomeIxpenseManager.Services;

public interface IJwtTokenService
{
    AuthResponse CreateToken(User user);
}
