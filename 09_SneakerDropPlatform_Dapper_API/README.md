# Sneaker Drop Platform (Dapper API)

Yüksek trafikli sınırlı sürüm ayakkabı satışları için hızlı çalışması amacıyla Dapper ve Repository Pattern kullanılarak tasarlanmış API servisi.

## 📷 Ekran Görüntüleri

*(Projenizi çalıştırdığınızda aldığınız ekran görüntülerini `docs/images` klasörüne atıp isimlerini `screenshot-1.png` gibi yaparak burada görünmesini sağlayabilirsiniz)*

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core Web API**
- **Dapper**
- **Repository Pattern**
- **Swagger**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Brand`
- `Drop`
- `Inventory`
- `Order`
- `Waitlist`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Veritabanını oluşturun ve `appsettings.json` içerisindeki bağlantı dizesini kendi SQL Server'ınıza göre düzenleyin.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
