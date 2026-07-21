# 4. Gün - Entity Sınıfları ve DbContext Yapısı

Bu klasör, 3. günde oluşturulan ASP.NET Core Web API altyapısının üzerine Entity Framework Core veri modelinin eklenmesini içerir.

## Bu aşamada yapılanlar

- `User`, `Category`, `Transaction` ve `RefreshToken` entity sınıfları oluşturuldu.
- Gelir ve gider ayrımı için `TransactionType` enum yapısı eklendi.
- Entity sınıfları arasındaki navigation property ilişkileri tanımlandı.
- `ApplicationDbContext` oluşturularak `DbSet` tanımları yapıldı.
- Tablo adları, alan uzunlukları, veri tipleri, indeksler ve ilişkiler Fluent API ile yapılandırıldı.
- SQL Server bağlantısı `Program.cs` üzerinden dependency injection sistemine kaydedildi.
- Örnek bağlantı cümlesi `appsettings.json` dosyasına eklendi.

## Entity ilişkileri

- Bir kullanıcının birden fazla kategorisi olabilir.
- Bir kullanıcının birden fazla gelir/gider işlemi olabilir.
- Bir kategori birden fazla işlemde kullanılabilir.
- Her işlem yalnızca kendi kullanıcısına ait ve aynı türdeki kategoriyle eşleştirilir.
- Bir kullanıcının birden fazla yenileme anahtarı olabilir.

## Derleme

```powershell
cd 4.Gün
dotnet restore
dotnet build
```
