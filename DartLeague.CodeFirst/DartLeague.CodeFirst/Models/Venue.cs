using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DartLeague.CodeFirst.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // e.g., Central Dart Arena

        // Navigation property (Matches played here)
        public ICollection<Match>? Matches { get; set; }

        // Teams that use this venue as their home
        public ICollection<Team>? HomeTeams { get; set; }

        // Visuals
        [StringLength(500)]
        public string? LogoUrl { get; set; }
    }
}