using CafePOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CafePOS.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // If there are tables, the db is already seeded
        if (context.Tables.Any())
        {
            return;
        }

        // 1. Seed Categories
        var categories = new Category[]
        {
            new Category { Name = "İçecekler" },
            new Category { Name = "Yiyecekler" },
            new Category { Name = "Tatlılar" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        // 2. Seed Products
        var products = new Product[]
        {
            new Product { Name = "Çay", Price = 15.00m, CategoryId = categories[0].Id },
            new Product { Name = "Türk Kahvesi", Price = 45.00m, CategoryId = categories[0].Id },
            new Product { Name = "Filtre Kahve", Price = 60.00m, CategoryId = categories[0].Id },
            new Product { Name = "Limonata", Price = 50.00m, CategoryId = categories[0].Id },
            
            new Product { Name = "Tost", Price = 60.00m, CategoryId = categories[1].Id },
            new Product { Name = "Hamburger", Price = 180.00m, CategoryId = categories[1].Id },
            new Product { Name = "Pizza", Price = 220.00m, CategoryId = categories[1].Id },
            new Product { Name = "Makarna", Price = 150.00m, CategoryId = categories[1].Id },
            
            new Product { Name = "Cheesecake", Price = 90.00m, CategoryId = categories[2].Id },
            new Product { Name = "Çikolatalı Pasta", Price = 110.00m, CategoryId = categories[2].Id },
            new Product { Name = "Sütlaç", Price = 70.00m, CategoryId = categories[2].Id }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // 3. Seed Tables
        var tables = new List<CafeTable>();
        for (int i = 1; i <= 15; i++)
        {
            tables.Add(new CafeTable { Name = $"Masa {i}", IsOccupied = false });
        }
        context.Tables.AddRange(tables);
        context.SaveChanges();

        // 4. Seed Historical Orders (Random past 6 months)
        var random = new Random();
        var historicalOrders = new List<Order>();
        var customerNames = new[] { "Ahmet Y.", "Ayşe K.", "Mehmet D.", "Fatma Ç.", "Ali C.", "Veli G.", "Zeynep Ş.", "Caner K.", "Eda A.", "Burak D." };

        var today = DateTime.Today;
        
        for (int i = 0; i < 800; i++)
        {
            var randomDaysAgo = random.Next(1, 180);
            var randomTime = new TimeSpan(random.Next(9, 23), random.Next(0, 59), 0);
            var orderDate = today.AddDays(-randomDaysAgo).Add(randomTime);
            
            var product = products[random.Next(products.Length)];
            var table = tables[random.Next(tables.Count)];
            var customer = random.Next(10) > 3 ? customerNames[random.Next(customerNames.Length)] : null; // 60% chance to have name
            
            historicalOrders.Add(new Order
            {
                TableId = table.Id,
                ProductId = product.Id,
                Quantity = random.Next(1, 4),
                CreatedDate = orderDate,
                IsPaid = true,
                PaidDate = orderDate.AddMinutes(random.Next(30, 120)),
                CustomerName = customer
            });
        }
        
        context.Orders.AddRange(historicalOrders);
        context.SaveChanges();
    }
}
