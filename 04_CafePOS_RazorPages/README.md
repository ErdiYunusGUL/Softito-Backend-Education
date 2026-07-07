# Cafe POS (Point of Sale)

Controller yapısı kullanılmadan doğrudan sayfa bazlı model (Page-Model) ile geliştirilmiş, kafeler için adisyon ve masa yönetim sistemi.

## 📷 Ekran Görüntüleri

*(Projenizi çalıştırdığınızda aldığınız ekran görüntülerini `docs/images` klasörüne atıp isimlerini `screenshot-1.png` gibi yaparak burada görünmesini sağlayabilirsiniz)*

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **SQL Server**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Product`
- `Category`
- `Order`
- `OrderItem`
- `Table`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Veritabanı migration'larını uyguladıktan sonra projeyi başlatabilirsiniz.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
