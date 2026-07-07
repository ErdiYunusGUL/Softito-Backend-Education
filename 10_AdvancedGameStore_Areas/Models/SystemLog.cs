using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedGameStore.Models
{
    public class SystemLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; }

        public string Details { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
