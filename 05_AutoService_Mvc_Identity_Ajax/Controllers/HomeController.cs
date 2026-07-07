using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoService_Mvc_Identity_Ajax.Models;
using Microsoft.AspNetCore.Authorization;
using AutoService_Mvc_Identity_Ajax.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService_Mvc_Identity_Ajax.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
        {
            // Admin Dashboard Verileri
            var totalLaborRevenue = await _context.ServiceActions.SumAsync(a => a.Price);
            var totalPartsRevenue = await _context.ServiceParts.SumAsync(p => p.Quantity * p.UnitPrice);
            
            ViewBag.TotalRevenue = totalLaborRevenue + totalPartsRevenue;
            
            ViewBag.MonthlyRecords = await _context.ServiceRecords
                .Where(r => r.EntryDate.Month == DateTime.Now.Month && r.EntryDate.Year == DateTime.Now.Year)
                .CountAsync();
                
            ViewBag.LowStockParts = await _context.Parts
                .Where(p => p.StockQuantity <= 10)
                .CountAsync();
                
            ViewBag.TotalRecords = await _context.ServiceRecords.CountAsync();
        }
        else
        {
            // Standart Kullanıcı (Usta) Verileri
            ViewBag.TodayRecords = await _context.ServiceRecords
                .Include(r => r.CarModel)
                .ThenInclude(m => m!.Brand)
                .Where(r => r.EntryDate.Date == DateTime.Today)
                .OrderByDescending(r => r.EntryDate)
                .ToListAsync();
        }

        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
