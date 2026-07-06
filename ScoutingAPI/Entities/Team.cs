using System.Collections.Generic;

namespace ScoutingAPI.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FoundedYear { get; set; }
        
        public int LeagueId { get; set; }
        public League League { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
