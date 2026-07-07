# Dart League Tracking System

Dart Ligi takımlarının, oyuncularının ve maç istatistiklerinin tutulduğu Code-First mimarili ASP.NET Core MVC projesi.

## 📷 Ekran Görüntüleri



![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **Entity Framework Core (Code First)**
- **SQL Server**
- **Bootstrap**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Player`
- `Team`
- `Match`
- `League`
- `MatchStats`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Veritabanını oluşturmak için Package Manager Console'da `Update-Database` çalıştırın veya terminalden `dotnet ef database update` komutunu girin.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
