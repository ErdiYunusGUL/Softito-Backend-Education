using FlightSearch.API.Data;
using FlightSearch.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly AppDbContext _context;

    public FlightsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/flights
    [HttpGet]
    public async Task<IActionResult> GetFlights([FromQuery] string? departureCity, [FromQuery] string? arrivalCity, [FromQuery] DateTime? departureDate, [FromQuery] string? ticketClass = "economy")
    {
        var query = _context.Flights
            .Include(f => f.Airline)
            .Include(f => f.DepartureAirport).ThenInclude(a => a.City)
            .Include(f => f.ArrivalAirport).ThenInclude(a => a.City)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(departureCity))
        {
            query = query.Where(f => f.DepartureAirport.City.Name.Contains(departureCity));
        }

        if (!string.IsNullOrWhiteSpace(arrivalCity))
        {
            query = query.Where(f => f.ArrivalAirport.City.Name.Contains(arrivalCity));
        }

        if (departureDate.HasValue)
        {
            query = query.Where(f => f.DepartureTime.Date == departureDate.Value.Date);
        }

        var flights = await query.ToListAsync();

        // Simulate External API integration (Mock)
        if (!string.IsNullOrWhiteSpace(departureCity) && !string.IsNullOrWhiteSpace(arrivalCity))
        {
            var mockExternalFlights = new List<Flight>
            {
                new Flight
                {
                    Id = 9991, // Fake ID
                    Airline = new Airline { Name = "Global Air (External)" },
                    DepartureAirport = new Airport { Name = "Intl", Code = "EXT1", City = new City { Name = departureCity, Country = new Country{ Name = "", Code = ""} } },
                    ArrivalAirport = new Airport { Name = "Intl", Code = "EXT2", City = new City { Name = arrivalCity, Country = new Country{ Name = "", Code = ""} } },
                    DepartureTime = departureDate ?? DateTime.Now.AddDays(1).Date.AddHours(14),
                    Price = 2500m
                },
                new Flight
                {
                    Id = 9992,
                    Airline = new Airline { Name = "SkyNet Airways" },
                    DepartureAirport = new Airport { Name = "Reg", Code = "EXT3", City = new City { Name = departureCity, Country = new Country{ Name = "", Code = ""} } },
                    ArrivalAirport = new Airport { Name = "Reg", Code = "EXT4", City = new City { Name = arrivalCity, Country = new Country{ Name = "", Code = ""} } },
                    DepartureTime = departureDate ?? DateTime.Now.AddDays(1).Date.AddHours(18),
                    Price = 1850m
                }
            };
            
            flights.AddRange(mockExternalFlights);
        }

        // Adjust prices based on class
        if (ticketClass?.ToLower() == "business")
        {
            foreach (var flight in flights)
            {
                flight.Price *= 2.5m;
            }
        }
        else if (ticketClass?.ToLower() == "firstclass")
        {
            foreach (var flight in flights)
            {
                flight.Price *= 4.0m;
            }
        }

        return Ok(flights);
    }
}
