namespace FlightSearch.API.Models;

public class Airline
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Flight> Flights { get; set; }
}
