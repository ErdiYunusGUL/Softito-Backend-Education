namespace FlightSearch.API.Models;

public class Airport
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; } // IST, SAW, ESB

    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
