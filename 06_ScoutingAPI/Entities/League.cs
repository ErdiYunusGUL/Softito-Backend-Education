using System.Collections.Generic;

namespace ScoutingAPI.Entities
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
