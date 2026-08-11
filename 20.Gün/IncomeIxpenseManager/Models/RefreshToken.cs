namespace IncomeIxpenseManager.Models;

public class RefreshToken
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public long? ReplacedByTokenId { get; set; }

    public User User { get; set; } = null!;
    public RefreshToken? ReplacedByToken { get; set; }
    public ICollection<RefreshToken> PreviousTokens { get; set; } = [];
}
