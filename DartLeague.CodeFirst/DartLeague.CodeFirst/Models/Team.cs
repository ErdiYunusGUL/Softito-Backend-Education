using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DartLeague.CodeFirst.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }

        // Navigation property
        public ICollection<Player>? Players { get; set; }

        // Home Venue
        [Required(ErrorMessage = "Lütfen bir ev sahibi mekan (bar) seçin.")]
        public int? HomeVenueId { get; set; }
        public Venue? HomeVenue { get; set; }

        // Visuals
        [StringLength(500)]
        public string? LogoUrl { get; set; }
    }
}