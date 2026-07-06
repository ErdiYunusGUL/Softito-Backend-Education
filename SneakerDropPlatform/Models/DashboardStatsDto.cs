using System.Collections.Generic;

namespace SneakerDropPlatform.Models
{
    public class DashboardStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItemsSold { get; set; }
        public List<TopSellerDto> TopSellers { get; set; } = new List<TopSellerDto>();
        public List<LowStockDto> LowStockAlerts { get; set; } = new List<LowStockDto>();
    }

    public class TopSellerDto
    {
        public string SneakerName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
    }

    public class LowStockDto
    {
        public string SneakerName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
