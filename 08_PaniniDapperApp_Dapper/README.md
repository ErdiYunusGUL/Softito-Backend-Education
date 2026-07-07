# Panini Çıkartma Koleksiyonu Platformu

Ağır bir ORM yerine performans odaklı Micro-ORM Dapper kullanılarak geliştirilmiş, koleksiyonerlerin çıkartma takaslayabildiği web uygulaması.

## 📷 Ekran Görüntüleri

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **Dapper (Micro-ORM)**
- **SQL Server**
- **Session Management**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `AlbumItem`
- `Collector`
- `Team`
- `Sticker`
- `TradeOffer`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Proje Dapper kullandığı için, veritabanı tablolarını oluşturacak script'i (örneğin SQL Server Management Studio üzerinden) çalıştırın.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
