# Kütüphane Yönetim Sistemi

Var olan bir veritabanı üzerinden Scaffold-DbContext kullanılarak oluşturulmuş (Database First) kütüphane ve ödünç alma yönetim sistemi.

## 📷 Ekran Görüntüleri



![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **Entity Framework Core (Database First)**
- **SQL Server**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Book`
- `Author`
- `Publisher`
- `Member`
- `Loan`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Proje Database-First olduğundan, mevcut veritabanı bağlantı dizesini `appsettings.json` dosyasında güncelleyerek projeyi başlatın.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
