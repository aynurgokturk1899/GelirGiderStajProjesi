using IncomeIxpenseManager.Data;
using IncomeIxpenseManager.DTOs.Auth;
using IncomeIxpenseManager.Models;
using IncomeIxpenseManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ApplicationDbContext dbContext, IJwtTokenService jwtTokenService)
    : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await dbContext.Users.AnyAsync(user => user.Email == normalizedEmail, cancellationToken))
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Kayıt oluşturulamadı.",
                Detail = "Bu e-posta adresi zaten kullanılıyor."
            });
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12)
        };

        dbContext.Users.Add(user);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Kayıt oluşturulamadı.",
                Detail = "Bu e-posta adresi zaten kullanılıyor."
            });
        }

        return StatusCode(StatusCodes.Status201Created, jwtTokenService.CreateToken(user));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);

        if (user is null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Giriş başarısız.",
                Detail = "E-posta adresi veya parola hatalı."
            });
        }

        return Ok(jwtTokenService.CreateToken(user));
    }
}
