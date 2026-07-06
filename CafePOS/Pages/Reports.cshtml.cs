using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CafePOS.Data;
using CafePOS.Models;

namespace CafePOS.Pages;

public class ReportsModel : PageModel
{
    private readonly AppDbContext _context;

    public ReportsModel(AppDbContext context)
    {
        _context = context;
    }

    public decimal TodayRevenue { get; set; }
    public int TodayOrdersCount { get; set; }
    public IList<TopProduct> TopProducts { get; set; } = new List<TopProduct>();
    public IList<TopProduct> WorstProducts { get; set; } = new List<TopProduct>();
    public IList<CategoryRevenue> RevenueByCategory { get; set; } = new List<CategoryRevenue>();
    public IList<MonthlyRevenue> RevenueByMonth { get; set; } = new List<MonthlyRevenue>();
    public IList<Order> RecentPaidOrders { get; set; } = new List<Order>();

    public class TopProduct
    {
        public string Name { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }

    public class CategoryRevenue
    {
        public string Name { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenue
    {
        public string MonthName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
    }

    public async Task OnGetAsync()
    {
        var today = DateTime.Today;

        // Today's summary
        var todaysPaidOrders = await _context.Orders
            .Include(o => o.Product)
            .Where(o => o.IsPaid && o.PaidDate >= today)
            .ToListAsync();

        TodayRevenue = todaysPaidOrders.Sum(o => o.Quantity * (o.Product?.Price ?? 0));
        TodayOrdersCount = todaysPaidOrders.Sum(o => o.Quantity);

        // Top 5 Products (All time paid)
        TopProducts = await _context.Orders
            .Where(o => o.IsPaid)
            .GroupBy(o => o.Product!.Name)
            .Select(g => new TopProduct { Name = g.Key, TotalQuantity = g.Sum(o => o.Quantity) })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(5)
            .ToListAsync();

        // Bottom 5 Products (All time paid)
        WorstProducts = await _context.Orders
            .Where(o => o.IsPaid)
            .GroupBy(o => o.Product!.Name)
            .Select(g => new TopProduct { Name = g.Key, TotalQuantity = g.Sum(o => o.Quantity) })
            .OrderBy(x => x.TotalQuantity)
            .Take(5)
            .ToListAsync();

        // Revenue By Category (All time paid)
        RevenueByCategory = await _context.Orders
            .Include(o => o.Product)
            .ThenInclude(p => p!.Category)
            .Where(o => o.IsPaid && o.Product != null && o.Product.Category != null)
            .GroupBy(o => o.Product!.Category!.Name)
            .Select(g => new CategoryRevenue { 
                Name = g.Key, 
                TotalRevenue = g.Sum(o => o.Quantity * o.Product!.Price) 
            })
            .ToListAsync();

        // Monthly Revenue
        var sixMonthsAgo = today.AddMonths(-6);
        var allOrdersForMonths = await _context.Orders
            .Include(o => o.Product)
            .Where(o => o.IsPaid && o.PaidDate >= sixMonthsAgo)
            .ToListAsync();

        RevenueByMonth = allOrdersForMonths
            .GroupBy(o => new { o.PaidDate!.Value.Year, o.PaidDate.Value.Month })
            .Select(g => new MonthlyRevenue 
            { 
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", new System.Globalization.CultureInfo("tr-TR")), 
                TotalRevenue = g.Sum(o => o.Quantity * o.Product!.Price) 
            })
            .OrderByDescending(x => DateTime.Parse(x.MonthName, new System.Globalization.CultureInfo("tr-TR")))
            .ToList();

        // Recent 20 Paid Orders
        RecentPaidOrders = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.Product)
            .Where(o => o.IsPaid)
            .OrderByDescending(o => o.PaidDate)
            .Take(20)
            .ToListAsync();
    }
}
