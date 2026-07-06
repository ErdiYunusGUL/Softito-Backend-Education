namespace FlightSearch.API.Models;

public class TicketType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}
