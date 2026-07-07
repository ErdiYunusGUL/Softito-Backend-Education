using System;
using System.Linq;
using AdvancedGameStore.Models;

namespace AdvancedGameStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any categories.
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category{Name="PlayStation", Description="PlayStation Games and Subscriptions"},
                new Category{Name="PC Oyunları", Description="PC Games (Steam, Epic)"},
                new Category{Name="E-Pin", Description="Digital Gift Cards and Game Currencies"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var products = new Product[]
            {
                new Product{Name="Valorant 2850 VP", Description="Valorant Points", Price=450, CategoryId=3, IconClass="fa-solid fa-gamepad", ImageUrl="https://via.placeholder.com/300x200/1e293b/10b981?text=Valorant+VP"},
                new Product{Name="Steam 10$ Cüzdan Kodu", Description="Steam Wallet Code", Price=320, CategoryId=3, IconClass="fa-brands fa-steam", ImageUrl="https://via.placeholder.com/300x200/1e293b/10b981?text=Steam+10$"},
                new Product{Name="PS Plus 1 Aylık", Description="PlayStation Plus Subscription", Price=200, CategoryId=1, IconClass="fa-brands fa-playstation", ImageUrl="https://via.placeholder.com/300x200/1e293b/10b981?text=PS+Plus"},
                new Product{Name="Cyberpunk 2077", Description="PC Steam Key", Price=800, CategoryId=2, IconClass="fa-solid fa-desktop", ImageUrl="https://via.placeholder.com/300x200/1e293b/10b981?text=Cyberpunk+2077"}
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            var customers = new Customer[]
            {
                new Customer{FullName="Ahmet Yılmaz", Email="ahmet@example.com"},
                new Customer{FullName="Ayşe Demir", Email="ayse@example.com"}
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var orders = new Order[]
            {
                new Order{CustomerId=1, ProductId=1, Quantity=1, TotalPrice=450, OrderDate=DateTime.UtcNow.AddDays(-1)},
                new Order{CustomerId=2, ProductId=3, Quantity=1, TotalPrice=200, OrderDate=DateTime.UtcNow.AddHours(-5)}
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            var tickets = new SupportTicket[]
            {
                new SupportTicket{CustomerId=1, Subject="VP Kodum Gelmedi", Message="Siparişim üzerinden 1 saat geçti hala kod gelmedi.", IsResolved=false},
                new SupportTicket{CustomerId=2, Subject="İade Talebi", Message="Yanlışlıkla aldım, iade etmek istiyorum.", IsResolved=true}
            };
            context.SupportTickets.AddRange(tickets);
            context.SaveChanges();

            var logs = new SystemLog[]
            {
                new SystemLog{Action="System Startup", Details="Application initialized and DB seeded."},
                new SystemLog{Action="User Login", Details="Admin user logged in."}
            };
            context.SystemLogs.AddRange(logs);
            context.SaveChanges();
        }
    }
}
