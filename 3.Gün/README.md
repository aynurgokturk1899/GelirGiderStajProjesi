# 3. Gün - ASP.NET Core Web API Projesinin Oluşturulması

Bu klasör yalnızca ASP.NET Core Web API projesinin oluşturulması ve ilerleyen aşamalarda kullanılacak gerekli paketlerin kurulması çalışmalarını içerir.

## Bu aşamada yapılanlar

- .NET 10 ASP.NET Core Web API projesi oluşturuldu.
- Proje solution dosyasına eklendi.
- Controller desteği etkinleştirildi.
- Swagger/OpenAPI yapılandırıldı.
- SQL Server, Entity Framework Core, JWT ve parola hashleme için gerekli NuGet paketleri kuruldu.
- Proje derlenerek Swagger ekranının açıldığı doğrulandı.

## Kurulu paketler

- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.10
- `Microsoft.EntityFrameworkCore.Design` 10.0.10
- `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.10
- `Microsoft.AspNetCore.OpenApi` 10.0.10
- `Microsoft.OpenApi` 2.7.5
- `Swashbuckle.AspNetCore` 10.2.3
- `BCrypt.Net-Next` 4.2.0

## Bu aşamaya dahil olmayanlar

- Entity sınıfları ve `DbContext`
- Migration ve veritabanı kurulumu
- Kayıt ve giriş endpoint'leri
- JWT üretme ve doğrulama kodları

## Çalıştırma

```powershell
cd 3.Gün
dotnet restore
dotnet run --project .\IncomeIxpenseManager\IncomeIxpenseManager.csproj
```

Swagger adresi:

```text
http://localhost:5000/swagger
```

Henüz controller oluşturulmadığı için Swagger ekranında `No operations defined in spec!` mesajının görünmesi normaldir.
