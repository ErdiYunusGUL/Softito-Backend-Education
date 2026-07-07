# Uçuş Arama & Rezervasyon Sistemi

API tabanlı bir backend ile bu API'yi tüketen bir MVC arayüzünün (Frontend) haberleştiği, JWT token korumalı uçuş rezervasyon uygulaması.

## 📷 Ekran Görüntüleri

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core Web API**
- **ASP.NET Core MVC**
- **JWT (JSON Web Token)**
- **Entity Framework Core**
- **HttpClient**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Flight`
- `Airport`
- `Booking`
- `City`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Önce `FlightSearch.API` projesini çalıştırın, ardından API URL'ini MVC projesinin `appsettings.json` dosyasına ekleyerek `FlightSearch.Web` projesini çalıştırın.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
