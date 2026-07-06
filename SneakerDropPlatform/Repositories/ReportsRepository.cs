using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SneakerDropPlatform.Models;

namespace SneakerDropPlatform.Repositories
{
    public class ReportsRepository
    {
        private readonly string _connectionString;

        public ReportsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var stats = new DashboardStatsDto();

                // 1. Total Revenue & Orders
                string summarySql = @"
                    SELECT 
                        ISNULL(SUM(TotalAmount), 0) AS TotalRevenue,
                        COUNT(Id) AS TotalOrders
                    FROM Orders;
                ";
                var summary = await connection.QuerySingleAsync(summarySql);
                stats.TotalRevenue = summary.TotalRevenue;
                stats.TotalOrders = summary.TotalOrders;

                // 2. Total Items Sold & Top Sellers
                string topSellersSql = @"
                    SELECT TOP 5
                        b.Name + ' ' + s.ModelName AS SneakerName,
                        SUM(oi.Quantity) AS TotalQuantitySold
                    FROM OrderItems oi
                    INNER JOIN Drops d ON oi.DropId = d.Id
                    INNER JOIN Sneakers s ON d.SneakerId = s.Id
                    INNER JOIN Brands b ON s.BrandId = b.Id
                    GROUP BY b.Name, s.ModelName
                    ORDER BY TotalQuantitySold DESC;
                ";
                var topSellers = await connection.QueryAsync<TopSellerDto>(topSellersSql);
                stats.TopSellers = topSellers.ToList();
                stats.TotalItemsSold = stats.TopSellers.Sum(x => x.TotalQuantitySold);

                // 3. Low Stock Alerts
                string lowStockSql = @"
                    SELECT 
                        b.Name + ' ' + s.ModelName AS SneakerName,
                        i.Size,
                        i.Quantity
                    FROM Inventory i
                    INNER JOIN Drops d ON i.DropId = d.Id
                    INNER JOIN Sneakers s ON d.SneakerId = s.Id
                    INNER JOIN Brands b ON s.BrandId = b.Id
                    WHERE i.Quantity <= 5
                    ORDER BY i.Quantity ASC;
                ";
                var lowStock = await connection.QueryAsync<LowStockDto>(lowStockSql);
                stats.LowStockAlerts = lowStock.ToList();

                return stats;
            }
        }
    }
}
