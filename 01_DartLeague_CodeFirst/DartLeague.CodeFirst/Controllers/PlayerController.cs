using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DartLeague.CodeFirst.Controllers
{
    public class PlayerController : Controller
    {
        private readonly AppDbContext _context;
        public PlayerController(AppDbContext context) { _context = context; }

        public async Task<IActionResult> Index() => View(await _context.Players.Include(p => p.Team).ToListAsync());

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.TeamId = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid) { _context.Add(player); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            ViewBag.TeamId = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();
            ViewBag.TeamId = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id) return NotFound();
            if (ModelState.IsValid) { _context.Update(player); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            ViewBag.TeamId = new SelectList(_context.Teams, "Id", "Name", player.TeamId);
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null) { _context.Players.Remove(player); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
