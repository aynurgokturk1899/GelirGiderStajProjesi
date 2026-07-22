using IncomeIxpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureTransaction(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();

        entity.ToTable("Users");
        entity.HasKey(user => user.Id);
        entity.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
        entity.Property(user => user.LastName).HasMaxLength(100).IsRequired();
        entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
        entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
        entity.Property(user => user.IsActive).HasDefaultValue(true);
        entity.Property(user => user.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");
        entity.HasIndex(user => user.Email).IsUnique();
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Category>();

        entity.ToTable("Categories");
        entity.HasKey(category => category.Id);
        entity.HasAlternateKey(category => new { category.Id, category.UserId, category.Type });
        entity.Property(category => category.Name).HasMaxLength(100).IsRequired();
        entity.Property(category => category.Type).HasConversion<byte>();
        entity.Property(category => category.IsActive).HasDefaultValue(true);
        entity.HasIndex(category => new { category.UserId, category.Name, category.Type }).IsUnique();

        entity.HasOne(category => category.User)
            .WithMany(user => user.Categories)
            .HasForeignKey(category => category.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Transaction>();

        entity.ToTable("Transactions");
        entity.HasKey(transaction => transaction.Id);
        entity.Property(transaction => transaction.Type).HasConversion<byte>();
        entity.Property(transaction => transaction.Amount).HasPrecision(18, 2);
        entity.Property(transaction => transaction.TransactionDate).HasColumnType("date");
        entity.Property(transaction => transaction.Description).HasMaxLength(500);
        entity.Property(transaction => transaction.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");
        entity.HasIndex(transaction => new { transaction.UserId, transaction.TransactionDate });

        entity.HasOne(transaction => transaction.User)
            .WithMany(user => user.Transactions)
            .HasForeignKey(transaction => transaction.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(transaction => transaction.Category)
            .WithMany(category => category.Transactions)
            .HasForeignKey(transaction => new { transaction.CategoryId, transaction.UserId, transaction.Type })
            .HasPrincipalKey(category => new { category.Id, category.UserId, category.Type })
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<RefreshToken>();

        entity.ToTable("RefreshTokens");
        entity.HasKey(token => token.Id);
        entity.Property(token => token.TokenHash).HasColumnType("varchar(64)").IsRequired();
        entity.Property(token => token.ExpiresAt).HasPrecision(0);
        entity.Property(token => token.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");
        entity.Property(token => token.RevokedDate).HasPrecision(0);
        entity.HasIndex(token => token.TokenHash).IsUnique();

        entity.HasOne(token => token.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(token => token.ReplacedByToken)
            .WithMany(token => token.PreviousTokens)
            .HasForeignKey(token => token.ReplacedByTokenId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
