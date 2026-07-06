using AutoService_Mvc_Identity_Ajax.Models;
using Microsoft.AspNetCore.Identity;

namespace AutoService_Mvc_Identity_Ajax.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Eğer baştan seed etmek istersek veritabanını temizleyip yeniden oluşturabiliriz, 
            // ama var olanın üstüne eklemeyi tercih edeceğiz. 
            context.Database.EnsureCreated();

            // Seed Roles
            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed Users
            if (!context.Users.Any())
            {
                var adminUser = new IdentityUser { UserName = "admin@servis.com", Email = "admin@servis.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, "Admin123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                var standardUser = new IdentityUser { UserName = "usta@servis.com", Email = "usta@servis.com", EmailConfirmed = true };
                result = await userManager.CreateAsync(standardUser, "Usta123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(standardUser, "User");
                }
            }

            // Seed Brands & Models
            if (!context.Brands.Any())
            {
                var brands = new List<Brand>
                {
                    new Brand { Name = "Toyota" },
                    new Brand { Name = "Honda" },
                    new Brand { Name = "Ford" },
                    new Brand { Name = "BMW" },
                    new Brand { Name = "Mercedes-Benz" },
                    new Brand { Name = "Audi" },
                    new Brand { Name = "Volkswagen" }
                };

                context.Brands.AddRange(brands);
                await context.SaveChangesAsync();

                var toyota = context.Brands.FirstOrDefault(b => b.Name == "Toyota")!.Id;
                var honda = context.Brands.FirstOrDefault(b => b.Name == "Honda")!.Id;
                var ford = context.Brands.FirstOrDefault(b => b.Name == "Ford")!.Id;
                var bmw = context.Brands.FirstOrDefault(b => b.Name == "BMW")!.Id;
                var mercedes = context.Brands.FirstOrDefault(b => b.Name == "Mercedes-Benz")!.Id;
                var audi = context.Brands.FirstOrDefault(b => b.Name == "Audi")!.Id;
                var vw = context.Brands.FirstOrDefault(b => b.Name == "Volkswagen")!.Id;

                var models = new List<CarModel>
                {
                    new CarModel { Name = "Corolla", BrandId = toyota },
                    new CarModel { Name = "Camry", BrandId = toyota },
                    new CarModel { Name = "Civic", BrandId = honda },
                    new CarModel { Name = "Accord", BrandId = honda },
                    new CarModel { Name = "Focus", BrandId = ford },
                    new CarModel { Name = "Mustang", BrandId = ford },
                    new CarModel { Name = "320i", BrandId = bmw },
                    new CarModel { Name = "520d", BrandId = bmw },
                    new CarModel { Name = "C200", BrandId = mercedes },
                    new CarModel { Name = "E250", BrandId = mercedes },
                    new CarModel { Name = "A4", BrandId = audi },
                    new CarModel { Name = "Q5", BrandId = audi },
                    new CarModel { Name = "Golf", BrandId = vw },
                    new CarModel { Name = "Passat", BrandId = vw }
                };

                context.CarModels.AddRange(models);
                await context.SaveChangesAsync();
            }

            // Seed Parts
            if (!context.Parts.Any())
            {
                var parts = new List<Part>
                {
                    new Part { Name = "Motor Yağı 5W-30 (4L)", PartCode = "OIL-5W30", StockQuantity = 150, UnitPrice = 850m },
                    new Part { Name = "Yağ Filtresi", PartCode = "FLT-OIL", StockQuantity = 200, UnitPrice = 150m },
                    new Part { Name = "Hava Filtresi", PartCode = "FLT-AIR", StockQuantity = 180, UnitPrice = 200m },
                    new Part { Name = "Polen Filtresi", PartCode = "FLT-POL", StockQuantity = 150, UnitPrice = 180m },
                    new Part { Name = "Ön Fren Balatası Takımı", PartCode = "BRK-PAD-F", StockQuantity = 80, UnitPrice = 1200m },
                    new Part { Name = "Arka Fren Balatası Takımı", PartCode = "BRK-PAD-R", StockQuantity = 70, UnitPrice = 900m },
                    new Part { Name = "Fren Diski (Adet)", PartCode = "BRK-DSC", StockQuantity = 40, UnitPrice = 1500m },
                    new Part { Name = "Akü 12V 60Ah", PartCode = "BAT-60A", StockQuantity = 30, UnitPrice = 2500m },
                    new Part { Name = "Akü 12V 72Ah", PartCode = "BAT-72A", StockQuantity = 25, UnitPrice = 3100m },
                    new Part { Name = "Buji Takımı (4'lü)", PartCode = "SPK-PLG-4", StockQuantity = 100, UnitPrice = 800m },
                    new Part { Name = "Silecek Takımı", PartCode = "WPR-BLD", StockQuantity = 120, UnitPrice = 450m },
                    new Part { Name = "Triger Kayışı Seti", PartCode = "TMB-SET", StockQuantity = 20, UnitPrice = 4500m },
                    new Part { Name = "V Kayışı", PartCode = "V-BLT", StockQuantity = 50, UnitPrice = 350m },
                    new Part { Name = "Debriyaj Seti", PartCode = "CLT-SET", StockQuantity = 15, UnitPrice = 5500m },
                    new Part { Name = "Antifriz (3L)", PartCode = "AFZ-3L", StockQuantity = 90, UnitPrice = 400m },
                    new Part { Name = "Rot Başı", PartCode = "STE-TIE", StockQuantity = 60, UnitPrice = 600m },
                    new Part { Name = "Z Rot", PartCode = "SUS-LNK", StockQuantity = 80, UnitPrice = 450m },
                    new Part { Name = "Amortisör (Adet)", PartCode = "SUS-SHK", StockQuantity = 40, UnitPrice = 1800m },
                    new Part { Name = "Termostat", PartCode = "CLL-TRM", StockQuantity = 35, UnitPrice = 750m },
                    new Part { Name = "Far Ampulü H7", PartCode = "LGT-H7", StockQuantity = 250, UnitPrice = 150m }
                };
                context.Parts.AddRange(parts);
                await context.SaveChangesAsync();
            }

            // Seed 50+ Historical Service Records
            if (context.ServiceRecords.Count() < 10)
            {
                var random = new Random();
                var carModels = context.CarModels.ToList();
                var parts = context.Parts.ToList();
                
                var actionNames = new[] { 
                    "Periyodik Bakım İşçiliği", "Fren Balata Değişimi", "Akü Değişim ve Ölçüm", 
                    "Triger Seti Değişim İşçiliği", "Bilgisayarlı Arıza Tespiti", "Alt Takım Kontrol ve Tamir",
                    "Klima Gazı Dolumu", "Far Ayarı", "Debriyaj Seti Değişimi", "Genel Kontrol (Check-up)"
                };

                var plates = new[] { "34ABC123", "06XYZ987", "35DEF456", "16BCA321", "07KJH765", "41POU098", "55TTR222", "61MNH444", "01ADA001", "34ZXC888" };
                
                var recordsToAdd = new List<ServiceRecord>();

                for (int i = 0; i < 60; i++)
                {
                    // Random date within last 6 months
                    int daysAgo = random.Next(1, 180);
                    var entryDate = DateTime.Now.AddDays(-daysAgo).AddHours(random.Next(-4, 4));
                    
                    var model = carModels[random.Next(carModels.Count)];
                    
                    var record = new ServiceRecord
                    {
                        CarModelId = model.Id,
                        LicensePlate = plates[random.Next(plates.Length)],
                        FaultDescription = "Periyodik bakım ve müşteri taleplerine istinaden genel kontroller yapıldı.",
                        EntryDate = entryDate
                    };

                    // Add 1 to 3 actions
                    int actionCount = random.Next(1, 4);
                    for (int j = 0; j < actionCount; j++)
                    {
                        var actionName = actionNames[random.Next(actionNames.Length)];
                        var price = random.Next(500, 2500);
                        
                        record.ServiceActions.Add(new ServiceAction
                        {
                            ActionName = actionName,
                            Price = price
                        });
                    }

                    // Add 1 to 4 parts
                    int partCount = random.Next(1, 5);
                    for (int j = 0; j < partCount; j++)
                    {
                        var part = parts[random.Next(parts.Count)];
                        var qty = (part.Name.Contains("Yağı") || part.Name.Contains("Antifriz")) ? 1 : random.Next(1, 3);
                        
                        // Prevent duplicate parts in the same record easily
                        if (!record.ServiceParts.Any(sp => sp.PartId == part.Id))
                        {
                            record.ServiceParts.Add(new ServicePart
                            {
                                PartId = part.Id,
                                Quantity = qty,
                                UnitPrice = part.UnitPrice
                            });
                            
                            // Decrease stock
                            part.StockQuantity -= qty;
                        }
                    }

                    recordsToAdd.Add(record);
                }

                context.ServiceRecords.AddRange(recordsToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
