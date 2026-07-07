# Softito Backend Education Projeleri

Bu depo, Softito Backend Eğitimi kapsamında C# ve ASP.NET Core ekosistemini baştan uca kavramak amacıyla geliştirilmiş **10 farklı projeyi** içermektedir.

Projeler; temel veritabanı işlemlerinden başlayarak, katmanlı mimari (N-Tier), kimlik doğrulama (Identity & JWT), mikro ORM (Dapper) ve API tabanlı modern mimarilere kadar geniş bir yelpazeyi kapsayacak şekilde adım adım tasarlanmıştır.

## 🛠️ Projeler ve Kullanılan Teknolojiler

### 1. [01_DartLeague_CodeFirst](01_DartLeague_CodeFirst)
**Konu:** Dart Ligi Takip Sistemi (Oyuncu, Takım, Maç ve İstatistik takibi)  
**Mimari & Teknolojiler:** ASP.NET Core MVC, Entity Framework Core (Code First), SQL Server.  
**Detay:** EF Core'un Code-First yaklaşımını temel alarak veritabanı tablolarının C# sınıflarından üretildiği, maç ve oyuncu istatistiklerinin tutulduğu temel seviye bir MVC projesidir.

### 2. [02_LibraryApp_DbFirst](02_LibraryApp_DbFirst)
**Konu:** Kütüphane Yönetim Sistemi (Kitap, Yazar, Üye ve Ödünç Alma süreçleri)  
**Mimari & Teknolojiler:** ASP.NET Core MVC, Entity Framework Core (Database First).  
**Detay:** Var olan bir veritabanından (Scaffold-DbContext ile) C# modellerinin (Entity'lerin) oluşturulduğu Database-First yaklaşımının öğretildiği projedir.

### 3. [03_GymApp_NTier_CodeFirst](03_GymApp_NTier_CodeFirst)
**Konu:** Spor Salonu Yönetimi (Üyelikler, Antrenörler, Dersler ve Antrenman Planları)  
**Mimari & Teknolojiler:** ASP.NET Core MVC, N-Tier (Çok Katmanlı Mimari), EF Core.  
**Detay:** Proje; `Web`, `Business` ve `DataAccess` olmak üzere katmanlara ayrılarak "Separation of Concerns" (Sorumlulukların Ayrılığı) prensibiyle geliştirilmiştir. Bağımlılık enjeksiyonu (Dependency Injection) yoğun olarak kullanılmıştır.

### 4. [04_CafePOS_RazorPages](04_CafePOS_RazorPages)
**Konu:** Kafe Satış ve Adisyon Sistemi (Masalar, Ürünler, Kategoriler ve Siparişler)  
**Mimari & Teknolojiler:** ASP.NET Core Razor Pages, EF Core.  
**Detay:** Geleneksel MVC yerine, Controller kullanmadan sayfa bazlı (Page-Model) bir geliştirme deneyimi sunan Razor Pages mimarisi kullanılarak oluşturulmuş bir Point of Sale (POS) uygulamasıdır.

### 5. [05_AutoService_Mvc_Identity_Ajax](05_AutoService_Mvc_Identity_Ajax)
**Konu:** Oto Servis ve Randevu Sistemi (Araçlar, Müşteriler, Randevular ve Faturalar)  
**Mimari & Teknolojiler:** ASP.NET Core MVC, ASP.NET Core Identity, AJAX, jQuery.  
**Detay:** Kullanıcı kayıt/giriş, yetkilendirme ve rol yönetimi için güçlü `Identity` kütüphanesinin kullanıldığı; sayfa yenilenmeden asenkron işlemler yapmak için yoğun olarak `AJAX` ve `jQuery` entegrasyonu barındıran projedir.

### 6. [06_ScoutingAPI](06_ScoutingAPI)
**Konu:** Futbolcu Gözlem (Scouting) Sistemi (Oyuncular, Yetenekler ve Gözlemci Raporları)  
**Mimari & Teknolojiler:** ASP.NET Core Web API, EF Core InMemory, Swagger.  
**Detay:** Sadece arayüzü olmayan, veri sağlayan bir RESTful API servisidir. Testleri hızlandırmak için `InMemory` veritabanı kullanılmıştır ve API dökümantasyonu için `Swagger` entegre edilmiştir.

### 7. [07_FlightSearchApp_API_MVC](07_FlightSearchApp_API_MVC)
**Konu:** Uçuş Arama ve Rezervasyon Sistemi  
**Mimari & Teknolojiler:** ASP.NET Core Web API & MVC, JWT (JSON Web Token), Cookie Authentication.  
**Detay:** İki ayrı projeden oluşur. Backend kısmı bir API olarak çalışır ve `JWT` ile korunur. Frontend kısmı ise bir MVC uygulamasıdır; API'ye HTTP istekleri (HttpClient) atarak veri çeker ve kullanıcılara sunar.

### 8. [08_PaniniDapperApp_Dapper](08_PaniniDapperApp_Dapper)
**Konu:** Panini Çıkartma Koleksiyonu ve Takas Platformu  
**Mimari & Teknolojiler:** ASP.NET Core MVC, Dapper (Micro-ORM), Session Management.  
**Detay:** EF Core gibi ağır bir ORM yerine performans odaklı bir Micro-ORM olan `Dapper` kullanılarak geliştirilmiştir. Ham SQL sorgularıyla (Raw SQL) veri erişim katmanı yazılmış ve kullanıcı oturumları için `Session` yapısı kullanılmıştır.

### 9. [09_SneakerDropPlatform_Dapper_API](09_SneakerDropPlatform_Dapper_API)
**Konu:** Sınırlı Üretim Ayakkabı (Sneaker) Çekiliş ve Satış Platformu  
**Mimari & Teknolojiler:** ASP.NET Core Web API, Dapper, Repository Pattern, Swagger.  
**Detay:** Yüksek trafik gerektiren "Sneaker Drop" (Anlık Satış) senaryolarına uygun olarak hız odaklı `Dapper` tercih edilmiştir. API tabanlıdır ve Repository Design Pattern uygulanarak kod tekrarı önlenmiştir.

### 10. [10_AdvancedGameStore_Areas](10_AdvancedGameStore_Areas)
**Konu:** Gelişmiş Oyun Mağazası ve Yönetim Paneli  
**Mimari & Teknolojiler:** ASP.NET Core MVC, Areas (Bölümler), EF Core.  
**Detay:** Büyük ölçekli MVC uygulamalarını modüllere ayırmak için kullanılan `Areas` mimarisiyle tasarlanmıştır. Normal kullanıcıların gördüğü mağaza arayüzü ile yöneticilerin gördüğü (Dashboard/Admin) arayüzleri mantıksal ve fiziksel olarak ayrılmıştır.

---
*Geliştirme Ortamı: .NET 10.0*
