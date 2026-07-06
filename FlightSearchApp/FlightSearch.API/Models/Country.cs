namespace FlightSearch.API.Models
{
    public class Country
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; } // e.g. TR, US
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
