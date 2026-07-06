using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DartLeague.CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalTeams = _context.Teams.Count();
            ViewBag.TotalPlayers = _context.Players.Count();
            ViewBag.TotalMatches = _context.Matches.Count();
            ViewBag.PlayedMatches = _context.Matches.Count(m => m.IsPlayed);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
