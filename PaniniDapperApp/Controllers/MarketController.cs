using Microsoft.AspNetCore.Mvc;
using PaniniDapperApp.Repositories;

namespace PaniniDapperApp.Controllers
{
    public class MarketController : Controller
    {
        private readonly StickerRepository _repository;

        public MarketController(StickerRepository repository)
        {
            _repository = repository;
        }

        private int? GetActiveCollectorId() => HttpContext.Session.GetInt32("ActiveCollectorId");

        public async Task<IActionResult> Index()
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            var listings = await _repository.GetMarketListingsAsync(collectorId.Value);
            return View(listings);
        }

        [HttpPost]
        public async Task<IActionResult> ListSticker(int stickerId)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            await _repository.CreateListingAsync(collectorId.Value, stickerId);
            TempData["Success"] = "Sticker placed on the market!";
            return RedirectToAction("MyListings");
        }

        public async Task<IActionResult> MyListings()
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            var myListings = await _repository.GetMyListingsAsync(collectorId.Value);
            var incomingOffers = await _repository.GetOffersForMyListingsAsync(collectorId.Value);

            ViewBag.IncomingOffers = incomingOffers;
            return View(myListings);
        }

        [HttpGet]
        public async Task<IActionResult> MakeOffer(int listingId, string playerName, string countryName)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            var myAlbum = await _repository.GetMyAlbumAsync(collectorId.Value);
            var myDuplicates = myAlbum.Where(x => x.Quantity > 1).ToList();

            ViewBag.ListingId = listingId;
            ViewBag.TargetPlayerName = playerName;
            ViewBag.TargetCountryName = countryName;

            return View(myDuplicates);
        }

        [HttpPost]
        public async Task<IActionResult> MakeOffer(int listingId, int offeredStickerId)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            await _repository.MakeOfferAsync(listingId, collectorId.Value, offeredStickerId);
            TempData["Success"] = "Offer sent successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            try
            {
                await _repository.AcceptOfferAsync(offerId, collectorId.Value);
                TempData["Success"] = "Trade accepted! Stickers have been exchanged.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error accepting trade: " + ex.Message;
            }
            return RedirectToAction("MyListings");
        }

        [HttpPost]
        public async Task<IActionResult> RejectOffer(int offerId)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            await _repository.RejectOfferAsync(offerId, collectorId.Value);
            TempData["Success"] = "Offer rejected.";
            return RedirectToAction("MyListings");
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveListing(int listingId)
        {
            var collectorId = GetActiveCollectorId();
            if (collectorId == null) return RedirectToAction("Login", "Account");

            await _repository.RemoveListingAsync(listingId, collectorId.Value);
            TempData["Success"] = "Listing removed.";
            return RedirectToAction("MyListings");
        }
    }
}
