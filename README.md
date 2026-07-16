# 💰 Kişisel Gelir-Gider Takip Uygulaması (Personal Income-Expense Tracker)

Kullanıcıların kişisel finanslarını yönetmelerine, günlük işlemlerini (gelir/gider) takip etmelerine, bütçelerini kategorilere ayırmalarına ve finansal durumlarını görselleştirmelerine yardımcı olmak için tasarlanmış uçtan uca (full-stack) bir web uygulamasıdır. Bu proje, **20 İş Günlük Yazılım Mühendisliği Staj Projesi** kapsamında geliştirilmiştir.

---

## 🚀 Proje Genel Bakışı

**Kişisel Gelir-Gider Takip Uygulaması**; arka uçta (backend) **ASP.NET Core Web API**, ön uçta (frontend) **Angular** ve **Entity Framework Core** aracılığıyla yönetilen bir **SQL Server** veritabanı kullanan, temiz mimari (clean architecture) prensiplerine uygun olarak geliştirilmiş güçlü bir web uygulamasıdır.

Bu projenin temel amacı; temiz kodlama ilkelerine, repository tasarım desenine, modern ön uç durum (state) ve form yönetimi standartlarına bağlı kalarak sıfırdan, bakımı kolay ve tamamen işlevsel kurumsal düzeyde bir uygulamayı başarıyla hayata geçirmektir.

### Temel Özellikler
* **Gelir & Gider Yönetimi:** Para akışını izlemek için eksiksiz CRUD (Ekleme, Okuma, Güncelleme, Silme) işlemleri.
* **Dinamik Kategori Yönetimi:** İşlem Türlerine (*Gelir / Gider*) göre dinamik olarak kategori oluşturma, listeleme ve filtreleme.
* **Gelişmiş Filtreleme:** İşlem geçmişini tarih aralıklarına ve kategorilere göre sorunsuz bir şekilde filtreleyebilme.
* **Canlı Finansal Gösterge Paneli (Dashboard):** *Toplam Gelir*, *Toplam Gider* ve *Kalan Bakiye* değerlerinin gerçek zamanlı hesaplanması ve görselleştirilmesi.
* **Veri Doğrulama & Hata Yönetimi:** Backend tarafında Fluent/DataAnnotation doğrulamaları; frontend tarafında ise dinamik reaktif form doğrulamaları ve kullanıcı dostu hata bildirimleri (toast alerts).
* **İlişkisel Veritabanı Eşleştirmesi:** Entity Framework Core kullanılarak güvenli silme kuralları (cascade rules) ile yapılandırılmış kategori-işlem ilişkisi.

---

## 🛠️ Teknoloji Yığını & Mimari

### Arka Uç (Backend - Web API)
* **Framework:** .NET 10.0 / ASP.NET Core Web API
* **Programlama Dili:** C#
* **ORM:** Entity Framework Core
* **Veritabanı:** MS SQL Server
* **Tasarım Desenleri:** Repository Deseni, DTO (Data Transfer Object) Deseni, Temiz Controller mimarisi.
* **API Dokümantasyonu:** Swagger / OpenAPI

### Ön Uç (Frontend - SPA)
* **Framework:** Angular (v17+)
* **Arayüz & Tasarım:** Tailwind CSS / Bootstrap (duyarlı ve profesyonel gösterge paneli tasarımı için)
* **Durum & Form Yönetimi:** Özel doğrulamalara sahip Reaktif Formlar (Reactive Forms)
* **HTTP İstemcisi:** Güçlü API entegrasyonu için RxJS observables mimarisi

## 🌐 API Uç Noktaları (Endpoints)

Arka uç, Swagger ile belgelenmiş RESTful API uç noktalarını dışarıya sunar:

| Metot | Adres | Açıklama |
| :--- | :--- | :--- |
| **GET** | `/api/transactions` | İsteğe bağlı tarih/kategori filtreleriyle tüm işlemleri listeler |
| **GET** | `/api/transactions/{id}` | Belirli bir işlemin detaylarını getirir |
| **POST** | `/api/transactions` | Yeni bir gelir/gider kaydı oluşturur |
| **PUT** | `/api/transactions/{id}` | Mevcut bir işlemi günceller |
| **DELETE** | `/api/transactions/{id}`| Bir işlemi siler |
| **GET** | `/api/categories` | Aktif kategorileri listeler |
| **POST** | `/api/categories` | Yeni bir kategori oluşturur |
| **GET** | `/api/dashboard/summary` | Agregasyon metriklerini getirir: Toplam Gelir, Toplam Gider, Kalan Bakiye |

---

## 📅 20 Günlük Geliştirme Yol Haritası

| Gün | Aşama | Tamamlanan Görevler |
| :---: | :--- | :--- |
| **1-2** | **Analiz & Tasarım** | Gereksinim analizi, DB Şemasının tasarlanması ve Git deposunun kurulması. |
| **3-5** | **Çekirdek Backend Kurulumu** | Web API projesinin oluşturulması, Entity tasarımı, DBContext yapılandırması ve Migration işlemleri. |
| **6-7** | **Kategori Servisleri** | Kategori CRUD uç noktalarının geliştirilmesi, validasyonlar ve veritabanı tohumlama (seeding). |
| **8-10**| **İşlem Servisleri** | Sıkı DTO eşleştirmesi ile eksiksiz İşlem (Transaction) CRUD mantığının yazılması. |
| **11-13**| **Filtreler & Dashboard**| Tarih/Kategori sorgu parametrelerinin entegrasyonu, Dashboard hesaplama mantığı ve genel Hata Yönetimi (Global Exception Handling). |
| **14-15**| **Ön Uç Çekirdeği** | Angular çalışma alanının kurulması, yönlendirme (routing) yapısı ve İşlem Tablosu bileşeni. |
| **16-17**| **Formlar & Yönetim** | Gerçek zamanlı arayüz doğrulamalarına sahip reaktif İşlem & Kategori ekleme/güncelleme formları. |
| **18**  | **Dashboard Entegrasyonu**| Dinamik hesaplamaları ve filtreleme kontrollerini gösteren duyarlı (responsive) gösterge paneli widget'ları. |
| **19-20**| **Kalite Kontrol & Teslim** | Kod iyileştirmeleri (isimlendirmelerin tamamen İngilizceye refaktör edilmesi), hata düzeltmeleri ve README dosyasının hazırlanması. |

---
