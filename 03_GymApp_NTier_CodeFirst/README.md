# Spor Salonu Yönetim Sistemi (N-Tier)

Spor salonu üyelerinin, antrenörlerin ve üyelik planlarının takip edildiği, Çok Katmanlı Mimari (N-Tier Architecture) prensipleriyle geliştirilmiş profesyonel bir otomasyon.

## 📷 Ekran Görüntüleri

*(Projenizi çalıştırdığınızda aldığınız ekran görüntülerini `docs/images` klasörüne atıp isimlerini `screenshot-1.png` gibi yaparak burada görünmesini sağlayabilirsiniz)*

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **N-Tier Architecture**
- **Entity Framework Core**
- **Dependency Injection**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Member`
- `Trainer`
- `Class`
- `Subscription`
- `WorkoutPlan`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Katmanlı mimari olduğundan, DataAccess katmanını başlangıç projesi olarak seçip veritabanını güncelleyin (`dotnet ef database update -s ../GymApp.NTier.Web/`).
4. Projeyi çalıştırın (F5 veya `dotnet run`).
