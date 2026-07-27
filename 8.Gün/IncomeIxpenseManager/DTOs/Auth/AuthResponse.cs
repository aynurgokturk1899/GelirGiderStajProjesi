namespace IncomeIxpenseManager.DTOs.Auth;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    AuthenticatedUserResponse User);

public sealed record AuthenticatedUserResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email);
