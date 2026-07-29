/*
    Kişisel Gelir-Gider Takip Uygulaması
    SQL Server başlangıç şeması

    Type değerleri:
      1 = Income (Gelir)
      2 = Expense (Gider)
*/

IF DB_ID(N'IncomeExpenseDb') IS NULL
BEGIN
    CREATE DATABASE IncomeExpenseDb;
END;
GO

USE IncomeExpenseDb;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id           int IDENTITY(1,1) NOT NULL,
        FirstName    nvarchar(100) NOT NULL,
        LastName     nvarchar(100) NOT NULL,
        Email        nvarchar(256) NOT NULL,
        PasswordHash nvarchar(500) NOT NULL,
        IsActive     bit NOT NULL
            CONSTRAINT DF_Users_IsActive DEFAULT (1),
        CreatedDate  datetime2(0) NOT NULL
            CONSTRAINT DF_Users_CreatedDate DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_Users PRIMARY KEY (Id),
        CONSTRAINT CK_Users_FirstName_NotBlank CHECK (LEN(LTRIM(RTRIM(FirstName))) > 0),
        CONSTRAINT CK_Users_LastName_NotBlank CHECK (LEN(LTRIM(RTRIM(LastName))) > 0),
        CONSTRAINT CK_Users_Email_NotBlank CHECK (LEN(LTRIM(RTRIM(Email))) > 0),
        CONSTRAINT CK_Users_Email_Format CHECK (Email LIKE N'%_@_%._%')
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_Users_Email'
      AND object_id = OBJECT_ID(N'dbo.Users')
)
BEGIN
    CREATE UNIQUE INDEX UX_Users_Email ON dbo.Users (Email);
END;
GO

IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories
    (
        Id       int IDENTITY(1,1) NOT NULL,
        UserId   int NOT NULL,
        Name     nvarchar(100) NOT NULL,
        [Type]   tinyint NOT NULL,
        IsActive bit NOT NULL
            CONSTRAINT DF_Categories_IsActive DEFAULT (1),

        CONSTRAINT PK_Categories PRIMARY KEY (Id),
        CONSTRAINT UQ_Categories_Id_UserId_Type UNIQUE (Id, UserId, [Type]),
        CONSTRAINT FK_Categories_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users (Id),
        CONSTRAINT CK_Categories_Name_NotBlank CHECK (LEN(LTRIM(RTRIM(Name))) > 0),
        CONSTRAINT CK_Categories_Type CHECK ([Type] IN (1, 2))
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_Categories_UserId_Name_Type'
      AND object_id = OBJECT_ID(N'dbo.Categories')
)
BEGIN
    CREATE UNIQUE INDEX UX_Categories_UserId_Name_Type
        ON dbo.Categories (UserId, Name, [Type]);
END;
GO

IF OBJECT_ID(N'dbo.Transactions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Transactions
    (
        Id              int IDENTITY(1,1) NOT NULL,
        UserId          int NOT NULL,
        CategoryId      int NOT NULL,
        [Type]          tinyint NOT NULL,
        Amount          decimal(18,2) NOT NULL,
        TransactionDate date NOT NULL,
        [Description]   nvarchar(500) NULL,
        CreatedDate     datetime2(0) NOT NULL
            CONSTRAINT DF_Transactions_CreatedDate DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_Transactions PRIMARY KEY (Id),
        CONSTRAINT FK_Transactions_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users (Id),
        CONSTRAINT FK_Transactions_Categories FOREIGN KEY (CategoryId, UserId, [Type])
            REFERENCES dbo.Categories (Id, UserId, [Type]),
        CONSTRAINT CK_Transactions_Type CHECK ([Type] IN (1, 2)),
        CONSTRAINT CK_Transactions_Amount_Positive CHECK (Amount > 0)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Transactions_UserId_TransactionDate'
      AND object_id = OBJECT_ID(N'dbo.Transactions')
)
BEGIN
    CREATE INDEX IX_Transactions_UserId_TransactionDate
        ON dbo.Transactions (UserId, TransactionDate DESC);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Transactions_UserId_CategoryId'
      AND object_id = OBJECT_ID(N'dbo.Transactions')
)
BEGIN
    CREATE INDEX IX_Transactions_UserId_CategoryId
        ON dbo.Transactions (UserId, CategoryId);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_Transactions_UserId_Type_TransactionDate'
      AND object_id = OBJECT_ID(N'dbo.Transactions')
)
BEGIN
    CREATE INDEX IX_Transactions_UserId_Type_TransactionDate
        ON dbo.Transactions (UserId, [Type], TransactionDate DESC)
        INCLUDE (Amount);
END;
GO

IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RefreshTokens
    (
        Id                bigint IDENTITY(1,1) NOT NULL,
        UserId            int NOT NULL,
        TokenHash         varchar(64) NOT NULL,
        ExpiresAt         datetime2(0) NOT NULL,
        CreatedDate       datetime2(0) NOT NULL
            CONSTRAINT DF_RefreshTokens_CreatedDate DEFAULT (SYSUTCDATETIME()),
        RevokedDate       datetime2(0) NULL,
        ReplacedByTokenId bigint NULL,

        CONSTRAINT PK_RefreshTokens PRIMARY KEY (Id),
        CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users (Id),
        CONSTRAINT FK_RefreshTokens_ReplacedByToken FOREIGN KEY (ReplacedByTokenId)
            REFERENCES dbo.RefreshTokens (Id),
        CONSTRAINT CK_RefreshTokens_ExpiresAt CHECK (ExpiresAt > CreatedDate),
        CONSTRAINT CK_RefreshTokens_RevokedDate CHECK
            (RevokedDate IS NULL OR RevokedDate >= CreatedDate)
    );
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_RefreshTokens_TokenHash'
      AND object_id = OBJECT_ID(N'dbo.RefreshTokens')
)
BEGIN
    CREATE UNIQUE INDEX UX_RefreshTokens_TokenHash
        ON dbo.RefreshTokens (TokenHash);
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_RefreshTokens_UserId_ExpiresAt'
      AND object_id = OBJECT_ID(N'dbo.RefreshTokens')
)
BEGIN
    CREATE INDEX IX_RefreshTokens_UserId_ExpiresAt
        ON dbo.RefreshTokens (UserId, ExpiresAt);
END;
GO
