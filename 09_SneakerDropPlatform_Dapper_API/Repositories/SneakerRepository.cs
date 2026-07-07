using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SneakerDropPlatform.Models;

namespace SneakerDropPlatform.Repositories
{
    public class SneakerRepository
    {
        private readonly string _connectionString;

        public SneakerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<IEnumerable<ActiveDropDto>> GetActiveDropsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        d.Id AS DropId,
                        b.Name AS BrandName,
                        s.ModelName,
                        s.Price,
                        s.ImageUrl,
                        d.DropDate,
                        ISNULL(SUM(i.Quantity), 0) AS TotalStock
                    FROM Drops d
                    INNER JOIN Sneakers s ON d.SneakerId = s.Id
                    INNER JOIN Brands b ON s.BrandId = b.Id
                    LEFT JOIN Inventory i ON d.Id = i.DropId
                    WHERE d.IsActive = 1
                    GROUP BY d.Id, b.Name, s.ModelName, s.Price, s.ImageUrl, d.DropDate
                    ORDER BY d.DropDate DESC;
                ";

                return await connection.QueryAsync<ActiveDropDto>(sql);
            }
        }

        public async Task<bool> JoinWaitlistAsync(int dropId, string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO Waitlists (DropId, Email)
                    VALUES (@DropId, @Email);
                ";

                int affectedRows = await connection.ExecuteAsync(sql, new { DropId = dropId, Email = email });
                return affectedRows > 0;
            }
        }

        public async Task<bool> CreateOrderAsync(CheckoutRequestDto request, decimal totalAmount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Order
                        string insertOrderSql = @"
                            INSERT INTO Orders (CustomerName, Email, TotalAmount)
                            OUTPUT INSERTED.Id
                            VALUES (@CustomerName, @Email, @TotalAmount);
                        ";
                        int orderId = await connection.QuerySingleAsync<int>(insertOrderSql, 
                            new { request.CustomerName, request.Email, TotalAmount = totalAmount }, 
                            transaction);

                        // 2. Insert OrderItems & Update Inventory
                        foreach (var item in request.Items)
                        {
                            string insertItemSql = @"
                                INSERT INTO OrderItems (OrderId, DropId, Quantity, Price)
                                VALUES (@OrderId, @DropId, @Quantity, @Price);
                            ";
                            await connection.ExecuteAsync(insertItemSql, 
                                new { OrderId = orderId, item.DropId, item.Quantity, item.Price }, 
                                transaction);

                            // Simple inventory decrement (deducting from any available size for simplicity)
                            // Note: Real apps would specify size in cart and deduct exactly that size.
                            string updateInventorySql = @"
                                WITH CTE AS (
                                    SELECT TOP (@Quantity) *
                                    FROM Inventory
                                    WHERE DropId = @DropId AND Quantity > 0
                                    ORDER BY Quantity DESC
                                )
                                UPDATE CTE
                                SET Quantity = Quantity - 1;
                            ";
                            
                            // Doing a basic loop to deduct total quantity across available stock
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                await connection.ExecuteAsync(updateInventorySql, 
                                    new { Quantity = 1, item.DropId }, 
                                    transaction);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
