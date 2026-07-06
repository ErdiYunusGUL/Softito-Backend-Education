using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DartLeague.CodeFirst.Controllers
{
    public class StandingsController : Controller
    {
        private readonly AppDbContext _context;

        public StandingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.ToListAsync();
            var matches = await _context.Matches.Where(m => m.IsPlayed).ToListAsync();

            var standings = teams.Select(t => new StandingViewModel
            {
                TeamName = t.Name,
                Played = matches.Count(m => m.HomeTeamId == t.Id || m.AwayTeamId == t.Id),
                Won = matches.Count(m => (m.HomeTeamId == t.Id && m.HomeTeamScore > m.AwayTeamScore) || (m.AwayTeamId == t.Id && m.AwayTeamScore > m.HomeTeamScore)),
                Drawn = matches.Count(m => (m.HomeTeamId == t.Id || m.AwayTeamId == t.Id) && m.HomeTeamScore == m.AwayTeamScore),
                Lost = matches.Count(m => (m.HomeTeamId == t.Id && m.HomeTeamScore < m.AwayTeamScore) || (m.AwayTeamId == t.Id && m.AwayTeamScore < m.HomeTeamScore)),
                PointsFor = matches.Where(m => m.HomeTeamId == t.Id).Sum(m => m.HomeTeamScore ?? 0) + matches.Where(m => m.AwayTeamId == t.Id).Sum(m => m.AwayTeamScore ?? 0),
                PointsAgainst = matches.Where(m => m.HomeTeamId == t.Id).Sum(m => m.AwayTeamScore ?? 0) + matches.Where(m => m.AwayTeamId == t.Id).Sum(m => m.HomeTeamScore ?? 0),
            }).ToList();

            foreach (var s in standings)
            {
                s.Points = (s.Won * 3) + (s.Drawn * 1);
            }

            standings = standings.OrderByDescending(s => s.Points).ThenByDescending(s => s.PointsFor - s.PointsAgainst).ToList();

            return View(standings);
        }
    }

    public class StandingViewModel
    {
        public string TeamName { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int PointsFor { get; set; }
        public int PointsAgainst { get; set; }
        public int GoalDifference => PointsFor - PointsAgainst;
        public int Points { get; set; }
    }
}
