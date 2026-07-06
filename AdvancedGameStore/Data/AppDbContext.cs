using Microsoft.EntityFrameworkCore;
using AdvancedGameStore.Models;

namespace AdvancedGameStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
    }
}
