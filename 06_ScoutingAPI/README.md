# Scouting (Futbolcu Gözlem) API

Futbolcu performans ve gözlem raporlarının tutulduğu, Swagger destekli ve InMemory veritabanı kullanan RESTful API projesi.

## 📷 Ekran Görüntüleri

*(Projenizi çalıştırdığınızda aldığınız ekran görüntülerini `docs/images` klasörüne atıp isimlerini `screenshot-1.png` gibi yaparak burada görünmesini sağlayabilirsiniz)*

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core Web API**
- **Entity Framework Core (InMemory)**
- **Swagger / OpenAPI**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Player`
- `Scout`
- `MatchReport`
- `Team`
- `Skill`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Veritabanı InMemory olduğu için ekstra bir SQL Server kurulumu gerektirmez, doğrudan çalıştırıp Swagger arayüzünü kullanabilirsiniz.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
