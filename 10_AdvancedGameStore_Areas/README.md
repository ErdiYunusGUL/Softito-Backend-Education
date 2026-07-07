# Advanced Game Store (Areas Mimarisi)

Büyük çaplı projelerde Admin ve Müşteri panellerini mantıksal olarak bölmek için kullanılan 'Areas' yapısı ile geliştirilmiş oyun satış mağazası.

## 📷 Ekran Görüntüleri

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **Areas Pattern**
- **Entity Framework Core**
- **SQL Server**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Product`
- `Category`
- `Customer`
- `Order`
- `SupportTicket`
- `SystemLog`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Migration'ları `Update-Database` komutuyla veritabanınıza uygulayın. Admin paneline erişmek için URL sonuna `/Admin` veya benzeri area routing'ini ekleyin.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
