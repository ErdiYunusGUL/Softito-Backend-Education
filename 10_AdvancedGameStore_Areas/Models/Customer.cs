using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedGameStore.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; }
        public ICollection<SupportTicket> SupportTickets { get; set; }
    }
}
