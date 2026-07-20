using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IncomeIxpenseManager.Configuration;
using IncomeIxpenseManager.DTOs.Auth;
using IncomeIxpenseManager.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IncomeIxpenseManager.Services;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;

    public AuthResponse CreateToken(User user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAtUtc,
            new AuthenticatedUserResponse(user.Id, user.FirstName, user.LastName, user.Email));
    }
}
