using FlightSearch.API.Data;
using FlightSearch.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (request == null) return BadRequest();

        var flight = await _context.Flights.FindAsync(request.FlightId);
        if (flight == null) return NotFound("Flight not found");

        var passenger = new Passenger
        {
            FirstName = request.PassengerFirstName,
            LastName = request.PassengerLastName
        };

        _context.Passengers.Add(passenger);

        var booking = new Booking
        {
            Flight = flight,
            Passenger = passenger,
            TicketTypeId = request.TicketTypeId // Default/selected
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Booking successful", BookingId = booking.Id, PNR = Guid.NewGuid().ToString().Substring(0, 6).ToUpper() });
    }
}

public class CreateBookingRequest
{
    public int FlightId { get; set; }
    public required string PassengerFirstName { get; set; }
    public required string PassengerLastName { get; set; }
    public string? PassportNumber { get; set; }
    public int TicketTypeId { get; set; } = 1; // Default to Economy
}
