using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DartLeague.CodeFirst.Controllers
{
    public class VenueController : Controller
    {
        private readonly AppDbContext _context;
        public VenueController(AppDbContext context) { _context = context; }

        public async Task<IActionResult> Index() => View(await _context.Venues.Include(v => v.HomeTeams).ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid) { _context.Add(venue); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.Id) return NotFound();
            if (ModelState.IsValid) { _context.Update(venue); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null) { _context.Venues.Remove(venue); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
