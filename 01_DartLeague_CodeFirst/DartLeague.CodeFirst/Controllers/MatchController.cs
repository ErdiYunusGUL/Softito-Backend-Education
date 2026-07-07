using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DartLeague.CodeFirst.Controllers
{
    public class MatchController : Controller
    {
        private readonly AppDbContext _context;
        public MatchController(AppDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var matches = await _context.Matches
                .Include(m => m.Venue)
                .Include(m => m.HomeTeam)
                    .ThenInclude(t => t.Category)
                .Include(m => m.AwayTeam)
                .ToListAsync();
            return View(matches);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name");
            ViewBag.HomeTeamId = new SelectList(_context.Teams, "Id", "Name");
            ViewBag.AwayTeamId = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Match match)
        {
            if (ModelState.IsValid)
            {
                if (match.HomeTeamId == match.AwayTeamId)
                {
                    ModelState.AddModelError("", "Home team and away team cannot be the same.");
                }
                else
                {
                    _context.Add(match);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", match.VenueId);
            ViewBag.HomeTeamId = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_context.Teams, "Id", "Name", match.AwayTeamId);
            return View(match);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var match = await _context.Matches.FindAsync(id);
            if (match == null) return NotFound();
            
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", match.VenueId);
            ViewBag.HomeTeamId = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_context.Teams, "Id", "Name", match.AwayTeamId);
            return View(match);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Match match)
        {
            if (id != match.Id) return NotFound();
            if (ModelState.IsValid)
            {
                if (match.HomeTeamId == match.AwayTeamId)
                {
                    ModelState.AddModelError("", "Home team and away team cannot be the same.");
                }
                else
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", match.VenueId);
            ViewBag.HomeTeamId = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_context.Teams, "Id", "Name", match.AwayTeamId);
            return View(match);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null) { _context.Matches.Remove(match); await _context.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.MatchGames)
                    .ThenInclude(g => g.HomePlayer1)
                .Include(m => m.MatchGames)
                    .ThenInclude(g => g.HomePlayer2)
                .Include(m => m.MatchGames)
                    .ThenInclude(g => g.AwayPlayer1)
                .Include(m => m.MatchGames)
                    .ThenInclude(g => g.AwayPlayer2)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) return NotFound();

            var vm = new DartLeague.CodeFirst.ViewModels.MatchDetailsViewModel
            {
                Match = match,
                Games = match.MatchGames?.OrderBy(g => g.GameOrder).ToList() ?? new System.Collections.Generic.List<MatchGame>()
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGames(int id)
        {
            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                    .ThenInclude(t => t.Players)
                .Include(m => m.AwayTeam)
                    .ThenInclude(t => t.Players)
                .Include(m => m.MatchGames)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) return NotFound();

            var vm = new DartLeague.CodeFirst.ViewModels.EditGamesViewModel
            {
                MatchId = match.Id,
                HomeTeamName = match.HomeTeam?.Name,
                AwayTeamName = match.AwayTeam?.Name,
                HomePlayers = match.HomeTeam?.Players?.ToList() ?? new System.Collections.Generic.List<Player>(),
                AwayPlayers = match.AwayTeam?.Players?.ToList() ?? new System.Collections.Generic.List<Player>()
            };

            for (int i = 1; i <= 9; i++)
            {
                var existingGame = match.MatchGames?.FirstOrDefault(g => g.GameOrder == i);
                vm.Games.Add(new DartLeague.CodeFirst.ViewModels.MatchGameViewModel
                {
                    Id = existingGame?.Id ?? 0,
                    GameOrder = i,
                    IsDoubles = i > 6, // 1-6 Singles, 7-9 Doubles
                    HomePlayer1Id = existingGame?.HomePlayer1Id,
                    HomePlayer2Id = existingGame?.HomePlayer2Id,
                    AwayPlayer1Id = existingGame?.AwayPlayer1Id,
                    AwayPlayer2Id = existingGame?.AwayPlayer2Id,
                    HomeScore = existingGame?.HomeScore ?? 0,
                    AwayScore = existingGame?.AwayScore ?? 0
                });
            }

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditGames(DartLeague.CodeFirst.ViewModels.EditGamesViewModel vm)
        {
            var match = await _context.Matches.Include(m => m.MatchGames).FirstOrDefaultAsync(m => m.Id == vm.MatchId);
            if (match == null) return NotFound();

            int totalHomeScore = 0;
            int totalAwayScore = 0;

            foreach (var gvm in vm.Games)
            {
                var game = match.MatchGames?.FirstOrDefault(g => g.GameOrder == gvm.GameOrder);
                if (game == null)
                {
                    game = new MatchGame { MatchId = match.Id, GameOrder = gvm.GameOrder };
                    _context.MatchGames.Add(game);
                }

                game.IsDoubles = gvm.IsDoubles;
                game.HomePlayer1Id = gvm.HomePlayer1Id;
                game.HomePlayer2Id = gvm.HomePlayer2Id;
                game.AwayPlayer1Id = gvm.AwayPlayer1Id;
                game.AwayPlayer2Id = gvm.AwayPlayer2Id;
                game.HomeScore = gvm.HomeScore;
                game.AwayScore = gvm.AwayScore;
                
                // Calculate match total score (1 pt for singles win, 2 pts for doubles win)
                int points = gvm.IsDoubles ? 2 : 1;
                if (gvm.HomeScore > gvm.AwayScore) totalHomeScore += points;
                else if (gvm.AwayScore > gvm.HomeScore) totalAwayScore += points;
            }
            
            match.HomeTeamScore = totalHomeScore;
            match.AwayTeamScore = totalAwayScore;
            match.IsPlayed = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
