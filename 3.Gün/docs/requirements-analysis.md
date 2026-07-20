# Gereksinim Analizi Raporu

## 1. Belge bilgileri

| Alan | Değer |
|---|---|
| Proje | Kişisel Gelir-Gider Takip Uygulaması |
| Kaynak belge | `Gelir_Gider_Takip_Projesi (1).pdf` |
| Hedef platform | Web uygulaması |
| Backend | ASP.NET Core Web API |
| Frontend | Angular |
| Veritabanı | SQL Server |
| Rapor tarihi | 16 Temmuz 2026 |

## 2. Projenin amacı

Projenin amacı, kullanıcıların günlük gelir ve giderlerini güvenli biçimde kaydedebildiği; kayıtlarını kategori ve tarih bazında inceleyebildiği; toplam gelir, toplam gider ve kalan bakiyesini görebildiği bir web uygulaması geliştirmektir.

Uygulama küçük ve yönetilebilir parçalar halinde geliştirilecek; çalışan backend, Angular arayüzü ve kalıcı SQL Server veritabanıyla uçtan uca tamamlanacaktır.

## 3. Kapsam

### 3.1 Zorunlu kapsam

- Gelir ve gider kayıtlarını ekleme, görüntüleme, güncelleme ve silme.
- Gelir ve gider kategorilerini yönetme.
- İşlemleri tarih aralığına ve kategoriye göre filtreleme.
- Toplam gelir, toplam gider ve kalan bakiye hesaplama.
- Formlarda doğrulama yapma ve anlaşılır hata mesajları gösterme.
- Verileri SQL Server üzerinde kalıcı olarak saklama.
- API işlemlerini Swagger veya Postman ile test edebilme.

### 3.2 Projeye dahil edilen opsiyonel kapsam

- Kullanıcı kayıt ve giriş ekranları.
- JWT access token ile kimlik doğrulama.
- Refresh token ile güvenli oturum yenileme.
- Aylık gelir-gider grafiği.
- İşlem listesinde sayfalama.

### 3.3 Kapsam dışı özellikler

- Banka hesaplarıyla otomatik veri alışverişi.
- Birden fazla para birimi ve kur dönüşümü.
- E-posta doğrulama ve şifre sıfırlama.
- Yönetici paneli ve kullanıcı yönetimi.
- Ortak aile hesabı veya kullanıcılar arasında kayıt paylaşımı.
- Dosya, fiş veya fatura yükleme.
- Bildirim ve bütçe limiti özellikleri.

Bu özellikler daha sonra eklenebilir ancak ilk teslimin kabulü için gerekli değildir.

## 4. Kullanıcı ve temel senaryo

Sistemin temel aktörü kayıtlı kullanıcıdır. Kullanıcı yalnızca kendi kategorilerine, işlemlerine ve oturum bilgilerine erişebilir.

Temel kullanım akışı:

1. Kullanıcı e-posta adresi ve parolasıyla kayıt olur.
2. Giriş yaptıktan sonra JWT ile korunan uygulama ekranlarına erişir.
3. Gelir veya gider kategorisi oluşturur.
4. Kategori, tür, tutar, tarih ve açıklama bilgileriyle işlem ekler.
5. İşlemleri listeler; tarih veya kategori filtresi uygular.
6. Gerekirse işlemi günceller ya da siler.
7. Ana ekranda toplam gelir, toplam gider ve kalan bakiyeyi görür.
8. Aylık grafikten gelir-gider değişimini inceler.

## 5. İşlevsel gereksinimler

### 5.1 Kimlik doğrulama

| Kod | Gereksinim | Öncelik |
|---|---|---|
| FR-AUTH-01 | Kullanıcı ad, soyad, benzersiz e-posta ve parola ile kayıt olabilmelidir. | Yüksek |
| FR-AUTH-02 | Kullanıcı e-posta ve parola ile giriş yapabilmelidir. | Yüksek |
| FR-AUTH-03 | Başarılı girişte kısa ömürlü JWT access token ve refresh token üretilmelidir. | Yüksek |
| FR-AUTH-04 | Access token süresi dolduğunda geçerli refresh token ile yeni token çifti alınabilmelidir. | Yüksek |
| FR-AUTH-05 | Kullanıcı çıkış yaptığında ilgili refresh token iptal edilmelidir. | Orta |
| FR-AUTH-06 | Hatalı veya süresi dolmuş token ile korunan kaynaklara erişim engellenmelidir. | Yüksek |

### 5.2 Kategori yönetimi

| Kod | Gereksinim | Öncelik |
|---|---|---|
| FR-CAT-01 | Kullanıcı kendi kategorilerini listeleyebilmelidir. | Yüksek |
| FR-CAT-02 | Kullanıcı gelir veya gider türünde yeni kategori ekleyebilmelidir. | Yüksek |
| FR-CAT-03 | Kullanıcı kendi kategorisinin adını ve durumunu güncelleyebilmelidir. | Yüksek |
| FR-CAT-04 | Kullanıcı artık kullanılmayacak kategoriyi pasifleştirebilmelidir. | Yüksek |
| FR-CAT-05 | İşlem formunda yalnızca seçilen işlem türüyle uyumlu aktif kategoriler gösterilmelidir. | Yüksek |

### 5.3 Gelir-gider işlemleri

| Kod | Gereksinim | Öncelik |
|---|---|---|
| FR-TRX-01 | Kullanıcı gelir veya gider kaydı ekleyebilmelidir. | Yüksek |
| FR-TRX-02 | Kullanıcı kendi işlemlerini listeleyebilmelidir. | Yüksek |
| FR-TRX-03 | Kullanıcı tek bir işlem kaydının detayını görüntüleyebilmelidir. | Yüksek |
| FR-TRX-04 | Kullanıcı kendi işlem kaydını güncelleyebilmelidir. | Yüksek |
| FR-TRX-05 | Kullanıcı kendi işlem kaydını silebilmelidir. | Yüksek |
| FR-TRX-06 | İşlemler başlangıç ve bitiş tarihine göre filtrelenebilmelidir. | Yüksek |
| FR-TRX-07 | İşlemler kategoriye ve işlem türüne göre filtrelenebilmelidir. | Yüksek |
| FR-TRX-08 | Filtreler birlikte kullanılabilmelidir. | Orta |
| FR-TRX-09 | İşlem listesi tarih sırasına göre sayfalanabilmelidir. | Orta |

### 5.4 Özet ve raporlama

| Kod | Gereksinim | Öncelik |
|---|---|---|
| FR-DASH-01 | Kullanıcının toplam geliri hesaplanmalıdır. | Yüksek |
| FR-DASH-02 | Kullanıcının toplam gideri hesaplanmalıdır. | Yüksek |
| FR-DASH-03 | Bakiye, `toplam gelir - toplam gider` formülüyle hesaplanmalıdır. | Yüksek |
| FR-DASH-04 | Özet değerleri seçilen tarih aralığına göre hesaplanabilmelidir. | Orta |
| FR-DASH-05 | Gelir ve giderler aylara göre gruplanarak grafikte gösterilmelidir. | Orta |

## 6. İş kuralları

| Kod | Kural |
|---|---|
| BR-01 | İşlem türü yalnızca `Income` veya `Expense` olabilir. |
| BR-02 | İşlem tutarı sıfırdan büyük olmalıdır; giderler negatif tutarla saklanmamalıdır. |
| BR-03 | Seçilen kategori işlemle aynı türe ve aynı kullanıcıya ait olmalıdır. |
| BR-04 | Aynı kullanıcı aynı türde aynı kategori adını birden fazla kez oluşturamamalıdır. |
| BR-05 | Pasif kategori yeni işlemlerde kullanılamamalı, geçmiş işlemlerde korunmalıdır. |
| BR-06 | Kullanıcı başka bir kullanıcıya ait kategori, işlem veya refresh token kaydına erişememelidir. |
| BR-07 | E-posta adresi sistem genelinde benzersiz olmalıdır. |
| BR-08 | Parolalar açık metin olarak kaydedilmemeli, güçlü bir parola hash algoritması kullanılmalıdır. |
| BR-09 | Ham refresh token veritabanında saklanmamalı; yalnızca token özeti saklanmalıdır. |
| BR-10 | Başlangıç tarihi bitiş tarihinden sonra olamaz. |
| BR-11 | Sayfa numarası ve sayfa boyutu pozitif olmalıdır; sayfa boyutuna üst sınır uygulanmalıdır. |
| BR-12 | Kayıt oluşturma zamanları UTC olarak saklanmalıdır. |

## 7. Veri gereksinimleri

Uygulama dört ana tablo kullanacaktır:

| Tablo | Amaç |
|---|---|
| `Users` | Kullanıcı hesabı ve parola hash bilgisini saklamak |
| `Categories` | Kullanıcıya ait gelir-gider kategorilerini saklamak |
| `Transactions` | Gelir ve gider kayıtlarını tek tabloda saklamak |
| `RefreshTokens` | JWT oturum yenileme ve token iptal geçmişini saklamak |

Ayrıntılı alanlar, ilişkiler ve indeksler [veritabanı tasarımı](database-design.md) belgesinde; çalıştırılabilir SQL Server şeması ise [`database/schema.sql`](../database/schema.sql) dosyasında yer alır.

## 8. API gereksinimleri

Planlanan temel endpoint'ler:

| Metot | Adres | Açıklama |
|---|---|---|
| `POST` | `/api/auth/register` | Yeni kullanıcı oluşturur |
| `POST` | `/api/auth/login` | Kullanıcı girişi yapar |
| `POST` | `/api/auth/refresh` | Token çiftini yeniler |
| `POST` | `/api/auth/logout` | Refresh tokenı iptal eder |
| `GET` | `/api/categories` | Kullanıcının kategorilerini listeler |
| `POST` | `/api/categories` | Yeni kategori oluşturur |
| `PUT` | `/api/categories/{id}` | Kategoriyi günceller |
| `DELETE` | `/api/categories/{id}` | Kategoriyi pasifleştirir |
| `GET` | `/api/transactions` | İşlemleri filtreli ve sayfalı listeler |
| `GET` | `/api/transactions/{id}` | İşlem detayını getirir |
| `POST` | `/api/transactions` | Yeni gelir veya gider ekler |
| `PUT` | `/api/transactions/{id}` | İşlemi günceller |
| `DELETE` | `/api/transactions/{id}` | İşlemi siler |
| `GET` | `/api/dashboard/summary` | Gelir, gider ve bakiye özetini getirir |
| `GET` | `/api/dashboard/monthly-chart` | Aylık grafik verisini getirir |

Kimlik doğrulama endpoint'leri dışındaki endpoint'ler geçerli JWT gerektirecektir.

## 9. Angular ekran gereksinimleri

| Ekran | Beklenen içerik |
|---|---|
| Kayıt | Ad, soyad, e-posta, parola ve parola tekrarı formu |
| Giriş | E-posta ve parola formu |
| Ana Sayfa / Özet | Toplam gelir, toplam gider, bakiye ve aylık grafik |
| İşlem Listesi | Sayfalı tablo, tarih, kategori ve tür filtreleri |
| Yeni İşlem | Tür, kategori, tutar, tarih ve açıklama formu |
| İşlem Güncelleme | Seçilen işlemin bilgileriyle doldurulmuş düzenleme formu |
| Kategori Yönetimi | Kategori listeleme, ekleme, düzenleme ve pasifleştirme |

## 10. Doğrulama ve hata mesajları

- Zorunlu alanlar hem Angular formunda hem API tarafında doğrulanmalıdır.
- E-posta biçimi ve benzersizliği kontrol edilmelidir.
- Parola için minimum güvenlik kuralları uygulanmalıdır.
- Tutar sıfırdan büyük olmalıdır.
- İşlem tarihi ve tarih aralığı geçerli olmalıdır.
- Kategori kullanıcıya ait, aktif ve işlem türüyle uyumlu olmalıdır.
- Hatalar standart bir API hata modeliyle ve kullanıcıya anlaşılır Türkçe mesajlarla dönmelidir.
- Kaynak bulunamadığında `404`, doğrulama hatasında `400`, kimlik doğrulama hatasında `401`, yetkisiz kaynak erişiminde `403` kullanılmalıdır.

## 11. İşlevsel olmayan gereksinimler

### Güvenlik

- Tüm kullanıcı verileri kullanıcı kimliğine göre izole edilmelidir.
- Parolalar ve tokenlar loglara yazılmamalıdır.
- Bağlantı bilgileri ve JWT anahtarı kaynak koda eklenmemelidir.
- Korunan API endpoint'leri JWT doğrulaması yapmalıdır.

### Performans

- İşlem listesi veritabanı seviyesinde sayfalanmalıdır.
- Tarih, kategori, kullanıcı ve tür filtrelerinde uygun indeksler kullanılmalıdır.
- Dashboard toplamları belleğe tüm kayıtlar alınmadan SQL sorgusuyla hesaplanmalıdır.

### Kullanılabilirlik

- Form hata mesajları ilgili alanın yanında gösterilmelidir.
- Gelir, gider ve bakiye değerleri tutarlı renk ve para biçiminde sunulmalıdır.
- Boş liste, yükleniyor ve hata durumları kullanıcıya açıkça gösterilmelidir.

### Bakım yapılabilirlik

- Sınıf, metot ve değişken adları anlamlı olmalıdır.
- Controller içinde uzun iş kuralları tutulmamalıdır.
- API giriş ve çıkışlarında entity yerine DTO kullanılması tercih edilmelidir.
- Tekrarlanan kod ortak bileşenlere ayrılmalıdır.

## 12. Kabul kriterleri

Proje aşağıdaki senaryolar hatasız tamamlandığında kabul edilebilir:

1. Yeni kullanıcı kayıt olabilir ve giriş yapabilir.
2. Giriş yapan kullanıcı kategori oluşturabilir.
3. Kullanıcı yeni gelir ve gider kaydı ekleyebilir.
4. İşlem kayıtları düzenlenebilir ve silinebilir.
5. İşlemler tarih ve kategoriye göre filtrelenebilir.
6. Sayfalama doğru toplam kayıt ve sayfa bilgisiyle çalışır.
7. Toplam gelir ve toplam gider doğru hesaplanır.
8. Bakiye, gelir eksi gider sonucuyla eşleşir.
9. Aylık grafik işlem kayıtlarıyla aynı toplamları gösterir.
10. Bir kullanıcı başka kullanıcının verilerine erişemez.
11. Access token yenileme ve çıkış işlemleri doğru çalışır.
12. Sayfa yenilendiğinde SQL Server'daki kayıtlar kaybolmaz.
13. API işlemleri Swagger veya Postman üzerinden test edilebilir.

## 13. Gereksinim izlenebilirliği

| Kaynak beklenti | Karşılayan gereksinimler | Doğrulama yöntemi |
|---|---|---|
| Gelir-gider CRUD | FR-TRX-01–05 | API ve arayüz entegrasyon testleri |
| Kategori yönetimi | FR-CAT-01–05 | API ve arayüz entegrasyon testleri |
| Tarih/kategori filtresi | FR-TRX-06–08 | Filtre sorgusu testleri |
| Gelir, gider ve bakiye | FR-DASH-01–04 | Bilinen örnek veriyle hesaplama testi |
| Form doğrulamaları | Bölüm 10 | Geçersiz giriş senaryoları |
| SQL Server kalıcılığı | Bölüm 7 | Uygulama yeniden başlatma testi |
| Kullanıcı ve JWT | FR-AUTH-01–06 | Kimlik doğrulama ve yetkilendirme testleri |
| Aylık grafik | FR-DASH-05 | API toplamı ile grafik karşılaştırması |
| Sayfalama | FR-TRX-09 | Farklı sayfa ve boyut senaryoları |

## 14. Varsayımlar ve riskler

### Varsayımlar

- İlk sürüm tek para birimiyle çalışacaktır; arayüzde Türk lirası kullanılacaktır.
- Uygulama internet tarayıcısından kullanılacaktır.
- Kullanıcı işlemleri kendi yerel tarihini girer, sistem oluşturma zamanlarını UTC saklar.
- Silinen işlem kaydı için ayrı bir denetim geçmişi tutulmayacaktır.

### Riskler ve önlemler

| Risk | Önlem |
|---|---|
| Kullanıcı verilerinin karışması | Her sorguda JWT'den alınan kullanıcı kimliğini filtre olarak kullanmak |
| Para hesaplama hataları | `decimal(18,2)` kullanmak ve örnek toplam testleri yazmak |
| Kategori ile işlem türünün uyuşmaması | API ve veritabanı ilişkisinde tür-kullanıcı uyumunu doğrulamak |
| Token hırsızlığı | Kısa ömürlü access token, hashlenmiş refresh token ve token rotasyonu kullanmak |
| Büyük işlem listesinde yavaşlama | Veritabanı seviyesinde sayfalama ve indeks kullanmak |
| Gizli bilgilerin Git'e eklenmesi | User Secrets veya ortam değişkenleri kullanmak ve `.gitignore` kontrolü yapmak |

## 15. Geliştirme sırası

PDF'deki 20 günlük plan korunacaktır:

- 1–2. gün: Analiz, gereksinim raporu ve veritabanı tasarımı.
- 3–5. gün: Web API, entity, `DbContext`, migration ve SQL Server kurulumu.
- 6–13. gün: Kategori, işlem, filtre, dashboard, doğrulama ve hata yönetimi.
- 14–18. gün: Angular ekranları, formlar, kategori yönetimi, özet ve filtreler.
- 19–20. gün: Test, hata düzeltme, kod temizliği, README ve sunum.

Opsiyonel kimlik doğrulama, grafik ve sayfalama özellikleri zorunlu akışları geciktirmeyecek şekilde ilgili backend ve frontend günlerine dağıtılacaktır.
