using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DartLeague.CodeFirst.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // e.g., Professional, Amateur

        // Navigation property for One-to-Many relationship
        public ICollection<Team>? Teams { get; set; }
    }
}