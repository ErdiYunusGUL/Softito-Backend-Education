namespace FlightSearch.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        
        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;
        
        public ICollection<Airport> Airports { get; set; } = new List<Airport>();
    }
}
