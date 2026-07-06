namespace FlightSearch.API.Models;

public class Passenger
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}
