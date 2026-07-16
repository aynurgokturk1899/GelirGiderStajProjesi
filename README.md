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

| Gün | Aşama | Planlanan Çalışma ve Teknik Detaylar |
| :---: | :--- | :--- |
| **1** | **Analiz** | **Gereksinim Analizi ve Sürüm Kontrolü:** Proje gereksinimlerinin detaylı analizi, yapılacakların önceliklendirilmesi ve GitHub üzerinde sürüm kontrol sisteminin kurulması. |
| **2** | **Analiz** | **Veritabanı Modelleme:** İlişkisel veritabanı şemasının tasarlanması, `Category` ve `Transaction` tabloları arasında bire-çok (1:N) ilişkinin kurulması. |
| **3** | **Backend** | **API Altyapısının Kurulması:** ASP.NET Core Web API projesinin oluşturulması; Entity Framework Core, SQL Server ve gerekli NuGet paketlerinin kurulması. |
| **4** | **Backend** | **Veritabanı Modellemesi (ORM):** Veritabanı tablolarına karşılık gelen C# Entity sınıflarının oluşturulması ve `DbContext` yapısının yapılandırılması. |
| **5** | **Backend** | **Veritabanı Göçü (Migration):** EF Core Migration mekanizması kullanılarak şemanın SQL Server üzerinde fiziksel veritabanına dönüştürülmesi ve kurulması. |
| **6** | **Backend** | **Kategori Listeleme & Ekleme Servisleri:** Kategori yönetimi için listeleme ve ekleme servislerinin, ilgili API uç noktalarının (Endpoints) geliştirilmesi. |
| **7** | **Backend** | **Kategori Güncelleme & Silme Servisleri:** Mevcut kategorilerin güncellenmesi ve silinmesi işlemlerinin kodlanarak kategori servislerinin tamamlanması. |
| **8** | **Backend** | **İşlem Ekleme Servisi:** Kullanıcının gelir veya gider kayıtlarını sisteme eklemesini sağlayan transaction ekleme servisinin geliştirilmesi. |
| **9** | **Backend** | **İşlem Listeleme & Detay Servisleri:** Eklenen tüm finansal işlemlerin listelenmesini ve tek bir işlemin detaylarının getirilmesini sağlayan servislerin kodlanması. |
| **10** | **Backend** | **İşlem Güncelleme & Silme Servisleri:** Gelir ve gider kayıtlarının güncellenmesi ve silinmesini sağlayan servislerin geliştirilerek tam işlem CRUD döngüsünün kurulması. |
| **11** | **Backend** | **Gelişmiş Filtreleme Altyapısı:** İşlemlerin tarih aralığına ve kategori türüne göre dinamik olarak filtrelenmesini sağlayan sorgu parametrelerinin API'ye entegre edilmesi. |
| **12** | **Backend** | **Dashboard Summary API:** Gösterge paneli için Toplam Gelir, Toplam Gider ve Kalan Bakiye değerlerini dinamik hesaplayarak dönecek olan özet API servisinin yazılması. |
| **13** | **Backend** | **Validasyon & Hata Yönetimi:** Model doğrulama kurallarının (Validation) tamamlanması ve uygulamada oluşabilecek hatalar için backend hata mesajlarının düzenlenmesi. |
| **14** | **Frontend** | **Angular SPA Kurulumu:** Angular projesinin oluşturulması, temel sayfa yapısının, yönlendirmelerin (Routing) ve arayüz kütüphanelerinin kurulması. |
| **15** | **Frontend** | **İşlem Listeleme Ekranı:** API'den gelen gelir-gider verilerinin reaktif (RxJS) olarak çekilmesi ve kullanıcıya dinamik bir tablo yapısında sunulacağı arayüzün geliştirilmesi. |
| **16** | **Frontend** | **Reaktif Form Yönetimi:** Yeni işlem ekleme ve mevcut işlemleri düzenleme işlemlerini yürütecek, reaktif doğrulamalara sahip formların geliştirilmesi. |
| **17** | **Frontend** | **Kategori Yönetim Ekranı:** Kategorilerin listelendiği, yeni kategori tanımlama ve güncelleme işlemlerinin yapılabildiği arayüz bileşenlerinin kodlanması. |
| **18** | **Frontend** | **Dashboard & Filtreleme UI:** Toplam gelir, gider ve bakiye kartlarını gösteren özet ekranın ve dinamik tarih/kategori filtreleme alanlarının geliştirilmesi. |
| **19** | **Teslim** | **Entegrasyon Testleri & Refaktör:** Frontend-Backend entegrasyonunun uçtan uca test edilmesi, hata ayıklama (Bug-Fix) süreçleri ve kod temizliğinin yapılması. |
| **20** | **Teslim** | **README & Proje Kapanışı:** GitHub üzerindeki README.md belgesinin son halinin hazırlanması, tüm teslim dosyalarının paketlenmesi ve 10 dakikalık teknik sunumun hazırlanması. |

---
