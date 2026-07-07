using System;

namespace SneakerDropPlatform.Models
{
    public class Waitlist
    {
        public int Id { get; set; }
        public int DropId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
