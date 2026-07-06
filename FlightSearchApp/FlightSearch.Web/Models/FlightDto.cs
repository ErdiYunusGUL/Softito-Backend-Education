namespace FlightSearch.Web.Models;

public class FlightDto
{
    public int Id { get; set; }
    public DateTime DepartureTime { get; set; }
    public decimal Price { get; set; }
    public AirlineDto Airline { get; set; }
    public AirportDto DepartureAirport { get; set; }
    public AirportDto ArrivalAirport { get; set; }
}

public class AirlineDto
{
    public string Name { get; set; }
}

public class AirportDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty; // For autocomplete
    public string CountryName { get; set; } = string.Empty; // For autocomplete
    public string DisplayText { get; set; } = string.Empty; // For autocomplete
    public CityDto? City { get; set; } // For Flights API mapping
}

public class CityDto
{
    public string Name { get; set; } = string.Empty;
}

public class CreateBookingDto
{
    public int FlightId { get; set; }
    public string PassengerFirstName { get; set; } = string.Empty;
    public string PassengerLastName { get; set; } = string.Empty;
    public int TicketTypeId { get; set; } = 1;
}
