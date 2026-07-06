using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoutingAPI.Data;
using ScoutingAPI.DTOs;
using ScoutingAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoutingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ScoutDbContext _context;

        public PlayersController(ScoutDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
        {
            var players = await _context.Players
                .Include(p => p.Team)
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position,
                    MarketValue = p.MarketValue,
                    TeamId = p.TeamId,
                    TeamName = p.Team != null ? p.Team.Name : string.Empty
                })
                .ToListAsync();

            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Include(p => p.Team)
                .Where(p => p.Id == id)
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Position = p.Position,
                    MarketValue = p.MarketValue,
                    TeamId = p.TeamId,
                    TeamName = p.Team != null ? p.Team.Name : string.Empty
                })
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(PlayerCreateDto dto)
        {
            var player = new Player
            {
                Name = dto.Name,
                Position = dto.Position,
                MarketValue = dto.MarketValue,
                TeamId = dto.TeamId
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                Position = player.Position,
                MarketValue = player.MarketValue,
                TeamId = player.TeamId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, PlayerCreateDto dto)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            player.Name = dto.Name;
            player.Position = dto.Position;
            player.MarketValue = dto.MarketValue;
            player.TeamId = dto.TeamId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
