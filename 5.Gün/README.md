# 5. Gün - Migration Oluşturma ve Veritabanını Kurma

Bu klasör, ilk dört günde hazırlanan ASP.NET Core Web API, Entity sınıfları ve
`ApplicationDbContext` yapısının üzerine Entity Framework Core migration'ının
oluşturulmasını ve SQL Server veritabanının kurulmasını içerir.

## Bu aşamada yapılanlar

- Entity Framework Core CLI aracı (`dotnet-ef`) yerel araç olarak tanımlandı.
- `InitialCreate` migration'ı oluşturuldu.
- Migration; `Users`, `Categories`, `Transactions` ve `RefreshTokens` tablolarını,
  ilişkileri, indeksleri ve doğrulama kısıtlarını içerir.
- Migration SQL Server'a uygulanarak `IncomeExpenseDb` veritabanı kuruldu.
- Kurulumu incelemek veya elle uygulamak için idempotent SQL betiği
  `database/initial-create.sql` altında üretildi.

## Ön koşullar

- .NET 10 SDK
- SQL Server
- `appsettings.json` içindeki bağlantı cümlesine erişebilen bir kullanıcı

Varsayılan bağlantı cümlesi Windows kimlik doğrulamasını kullanır:

```text
Server=localhost;Database=IncomeExpenseDb;Trusted_Connection=True;TrustServerCertificate=True;
```

SQL Server örneğiniz farklıysa `Server` değerini kendi ortamınıza göre düzenleyin.

## Kurulum komutları

`5.Gün` klasöründe çalıştırın:

```powershell
dotnet tool restore
dotnet restore
dotnet ef database update --project IncomeIxpenseManager
```

Migration listesini kontrol etmek için:

```powershell
dotnet ef migrations list --project IncomeIxpenseManager
```

Veritabanını migration öncesine döndürmek için:

```powershell
dotnet ef database update 0 --project IncomeIxpenseManager
```

> `database/initial-create.sql` idempotent üretildiği için hedef veritabanının
> migration durumunu kontrol ederek yalnızca eksik adımları uygular.

## Klasör yapısı

```text
5.Gün/
├── IncomeIxpenseManager/
│   ├── Data/ApplicationDbContext.cs
│   ├── Models/
│   ├── Migrations/
│   ├── Program.cs
│   └── appsettings.json
├── database/initial-create.sql
├── dotnet-tools.json
└── IncomeIxpenseManager.sln
```

Bu aşamaya repository, servis sınıfları ve CRUD endpoint'leri dahil değildir.
