using Microsoft.AspNetCore.Mvc;
using PaniniDapperApp.Repositories;

namespace PaniniDapperApp.Controllers
{
    public class StickersController : Controller
    {
        private readonly StickerRepository _repository;

        public StickersController(StickerRepository repository)
        {
            _repository = repository;
        }

        private int? GetActiveCollectorId()
        {
            return HttpContext.Session.GetInt32("ActiveCollectorId");
        }

        public async Task<IActionResult> Index()
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            var myAlbum = await _repository.GetMyAlbumAsync(collectorId.Value);
            var missingStickers = await _repository.GetMissingStickersAsync(collectorId.Value);

            ViewBag.MissingStickers = missingStickers;
            
            return View(myAlbum);
        }
    }
}
