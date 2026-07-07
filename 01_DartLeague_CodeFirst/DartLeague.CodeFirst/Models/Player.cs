using System.ComponentModel.DataAnnotations;

namespace DartLeague.CodeFirst.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Foreign Key
        public int TeamId { get; set; }

        // Navigation property
        public Team? Team { get; set; }
    }
}