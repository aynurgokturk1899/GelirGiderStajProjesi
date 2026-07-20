# 3. Gün - ASP.NET Core Web API

Bu klasör kişisel gelir-gider takip uygulamasının ASP.NET Core Web API başlangıç çalışmasını içerir.

## Tamamlanan çalışmalar

- .NET 10 ASP.NET Core Web API projesinin oluşturulması
- SQL Server ve Entity Framework Core paketlerinin kurulması
- Swagger/OpenAPI yapılandırması
- `User`, `Category`, `Transaction` ve `RefreshToken` entity sınıfları
- `ApplicationDbContext`, ilişkiler, indeksler ve kısıtlamalar
- `InitialCreate` migration'ı
- Kullanıcı kayıt ve giriş endpoint'leri
- BCrypt parola hashleme
- JWT access token üretimi ve doğrulaması
- Swagger JWT `Authorize` desteği

## Kurulu temel paketler

- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.10
- `Microsoft.EntityFrameworkCore.Design` 10.0.10
- `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.10
- `Microsoft.AspNetCore.OpenApi` 10.0.10
- `Swashbuckle.AspNetCore` 10.2.3
- `BCrypt.Net-Next` 4.2.0

## İlk kurulum

```powershell
cd 3.Gün
dotnet restore
dotnet tool restore
```

JWT anahtarı güvenlik nedeniyle repoya eklenmemiştir. Yerel geliştirme ortamında oluşturmak için:

```powershell
$keyBytes = [byte[]]::new(32)
[System.Security.Cryptography.RandomNumberGenerator]::Fill($keyBytes)
$jwtSecret = [Convert]::ToBase64String($keyBytes)
dotnet user-secrets set "Jwt:Key" $jwtSecret --project .\IncomeIxpenseManager\IncomeIxpenseManager.csproj
```

SQL Server bağlantısı gerekirse `IncomeIxpenseManager/appsettings.json` içindeki `DefaultConnection` üzerinden düzenlenir.

## Migration ve çalıştırma

```powershell
dotnet tool run dotnet-ef database update --project .\IncomeIxpenseManager
dotnet run --project .\IncomeIxpenseManager\IncomeIxpenseManager.csproj
```

Swagger adresi:

```text
http://localhost:5000/swagger
```

Auth endpoint'leri:

```text
POST /api/auth/register
POST /api/auth/login
```
