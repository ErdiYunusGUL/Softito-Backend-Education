using AutoService_Mvc_Identity_Ajax.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService_Mvc_Identity_Ajax.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            // 1. Markalara Göre Araç Dağılımı
            var brandDistribution = await _context.ServiceRecords
                .Include(s => s.CarModel)
                .ThenInclude(m => m!.Brand)
                .GroupBy(s => s.CarModel!.Brand!.Name)
                .Select(g => new { BrandName = g.Key, Count = g.Count() })
                .ToListAsync();

            var brandLabels = brandDistribution.Select(b => b.BrandName).ToList();
            var brandData = brandDistribution.Select(b => b.Count).ToList();

            // 2. En Çok Yapılan İşlemler (Top 5)
            var topActions = await _context.ServiceActions
                .GroupBy(a => a.ActionName)
                .Select(g => new { ActionName = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var actionLabels = topActions.Select(a => a.ActionName).ToList();
            var actionData = topActions.Select(a => a.Count).ToList();

            return Json(new
            {
                BrandChart = new { labels = brandLabels, data = brandData },
                ActionChart = new { labels = actionLabels, data = actionData }
            });
        }
    }
}
