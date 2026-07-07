using FlightSearch.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AirportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AirportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(new List<object>());

        var airports = await _context.Airports
            .Include(a => a.City)
            .ThenInclude(c => c.Country)
            .Where(a => a.Name.Contains(q) || a.City.Name.Contains(q) || a.Code.Contains(q))
            .Select(a => new
            {
                a.Id,
                a.Name,
                a.Code,
                CityName = a.City.Name,
                CountryName = a.City.Country.Name,
                DisplayText = $"{a.City.Name} ({a.Code}) - {a.Name}, {a.City.Country.Name}"
            })
            .Take(10)
            .ToListAsync();

        return Ok(airports);
    }
}
