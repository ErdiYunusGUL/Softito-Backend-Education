# Oto Servis & Randevu Sistemi

Araçların bakım kayıtlarının tutulduğu, Identity ile kullanıcı doğrulamasının yapıldığı ve asenkron veri işlemleri için yoğun AJAX kullanılmış bir otomasyon.

## 📷 Ekran Görüntüleri

*(Projenizi çalıştırdığınızda aldığınız ekran görüntülerini `docs/images` klasörüne atıp isimlerini `screenshot-1.png` gibi yaparak burada görünmesini sağlayabilirsiniz)*

![Ana Ekran](docs/images/screenshot-1.png)
<br/>
![Detay Ekranı](docs/images/screenshot-2.png)

## 🚀 Kullanılan Teknolojiler

- **ASP.NET Core MVC**
- **ASP.NET Core Identity**
- **AJAX & jQuery**
- **Entity Framework Core**

## 🗂 Temel Modeller (Entities)

Projeyi oluşturan ana nesneler şunlardır:
- `Vehicle`
- `Customer`
- `Appointment`
- `ServiceRecord`
- `Invoice`
- `Mechanic`

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi Visual Studio veya Visual Studio Code ile açın.
2. `appsettings.json` dosyası içerisindeki veritabanı bağlantı cümlenizi (Connection String) kendi bilgisayarınıza göre güncelleyin.
3. Identity tablolarının oluşturulması için migration işlemini gerçekleştirip `Update-Database` komutunu çalıştırın.
4. Projeyi çalıştırın (F5 veya `dotnet run`).
