using Microsoft.AspNetCore.Mvc;
using PaniniDapperApp.Repositories;

namespace PaniniDapperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly StickerRepository _repository;

        public HomeController(StickerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("ActiveCollectorId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var stats = await _repository.GetDashboardStatsAsync();
            var topCollectors = await _repository.GetTopCollectorsAsync();
            var topStickers = await _repository.GetMostTradedStickersAsync();

            ViewBag.TopCollectors = topCollectors;
            ViewBag.TopStickers = topStickers;

            return View(stats);
        }
    }
}
