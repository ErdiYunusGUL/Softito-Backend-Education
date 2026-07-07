namespace FlightSearch.API.Models;

public class Flight
{
    public int Id { get; set; }
    public int DepartureAirportId { get; set; }
    public Airport DepartureAirport { get; set; }
    public int ArrivalAirportId { get; set; }
    public Airport ArrivalAirport { get; set; }
    public int AirlineId { get; set; }
    public Airline Airline { get; set; }
    public DateTime DepartureTime { get; set; }
    public decimal Price { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}
