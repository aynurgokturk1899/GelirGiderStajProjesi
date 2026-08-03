using IncomeIxpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IncomeIxpenseManager.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Models.Transaction> Transactions => Set<Models.Transaction>();
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

        entity.ToTable("Users", table =>
        {
            table.HasCheckConstraint("CK_Users_FirstName_NotBlank", "LEN(LTRIM(RTRIM([FirstName]))) > 0");
            table.HasCheckConstraint("CK_Users_LastName_NotBlank", "LEN(LTRIM(RTRIM([LastName]))) > 0");
            table.HasCheckConstraint("CK_Users_Email_NotBlank", "LEN(LTRIM(RTRIM([Email]))) > 0");
            table.HasCheckConstraint("CK_Users_Email_Format", "[Email] LIKE N'%_@_%._%'");
        });

        entity.HasKey(user => user.Id);
        entity.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
        entity.Property(user => user.LastName).HasMaxLength(100).IsRequired();
        entity.Property(user => user.Email).HasMaxLength(256).IsRequired();
        entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
        entity.Property(user => user.IsActive).HasDefaultValue(true);
        entity.Property(user => user.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");
        entity.HasIndex(user => user.Email).IsUnique().HasDatabaseName("UX_Users_Email");
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Category>();

        entity.ToTable("Categories", table =>
        {
            table.HasCheckConstraint("CK_Categories_Name_NotBlank", "LEN(LTRIM(RTRIM([Name]))) > 0");
            table.HasCheckConstraint("CK_Categories_Type", "[Type] IN (1, 2)");
        });

        entity.HasKey(category => category.Id);
        entity.HasAlternateKey(category => new { category.Id, category.UserId, category.Type })
            .HasName("UQ_Categories_Id_UserId_Type");
        entity.Property(category => category.Name).HasMaxLength(100).IsRequired();
        entity.Property(category => category.Type).HasConversion<byte>();
        entity.Property(category => category.IsActive).HasDefaultValue(true);
        entity.HasIndex(category => new { category.UserId, category.Name, category.Type })
            .IsUnique()
            .HasDatabaseName("UX_Categories_UserId_Name_Type");

        entity.HasOne(category => category.User)
            .WithMany(user => user.Categories)
            .HasForeignKey(category => category.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Models.Transaction>();

        entity.ToTable("Transactions", table =>
        {
            table.HasCheckConstraint("CK_Transactions_Type", "[Type] IN (1, 2)");
            table.HasCheckConstraint("CK_Transactions_Amount_Positive", "[Amount] > 0");
        });

        entity.HasKey(transaction => transaction.Id);
        entity.Property(transaction => transaction.Type).HasConversion<byte>();
        entity.Property(transaction => transaction.Amount).HasPrecision(18, 2);
        entity.Property(transaction => transaction.TransactionDate).HasColumnType("date");
        entity.Property(transaction => transaction.Description).HasMaxLength(500);
        entity.Property(transaction => transaction.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");

        entity.HasIndex(transaction => new { transaction.UserId, transaction.TransactionDate })
            .IsDescending(false, true)
            .HasDatabaseName("IX_Transactions_UserId_TransactionDate");
        entity.HasIndex(transaction => new { transaction.UserId, transaction.CategoryId })
            .HasDatabaseName("IX_Transactions_UserId_CategoryId");
        entity.HasIndex(transaction => new { transaction.UserId, transaction.Type, transaction.TransactionDate })
            .IsDescending(false, false, true)
            .IncludeProperties(transaction => transaction.Amount)
            .HasDatabaseName("IX_Transactions_UserId_Type_TransactionDate");

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

        entity.ToTable("RefreshTokens", table =>
        {
            table.HasCheckConstraint("CK_RefreshTokens_ExpiresAt", "[ExpiresAt] > [CreatedDate]");
            table.HasCheckConstraint(
                "CK_RefreshTokens_RevokedDate",
                "[RevokedDate] IS NULL OR [RevokedDate] >= [CreatedDate]");
        });

        entity.HasKey(token => token.Id);
        entity.Property(token => token.TokenHash).HasColumnType("varchar(64)").IsRequired();
        entity.Property(token => token.ExpiresAt).HasPrecision(0);
        entity.Property(token => token.CreatedDate).HasPrecision(0).HasDefaultValueSql("SYSUTCDATETIME()");
        entity.Property(token => token.RevokedDate).HasPrecision(0);
        entity.HasIndex(token => token.TokenHash).IsUnique().HasDatabaseName("UX_RefreshTokens_TokenHash");
        entity.HasIndex(token => new { token.UserId, token.ExpiresAt })
            .HasDatabaseName("IX_RefreshTokens_UserId_ExpiresAt");

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
