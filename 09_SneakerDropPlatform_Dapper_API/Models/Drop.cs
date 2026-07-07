using System;

namespace SneakerDropPlatform.Models
{
    public class Drop
    {
        public int Id { get; set; }
        public int SneakerId { get; set; }
        public DateTime DropDate { get; set; }
        public bool IsActive { get; set; }
    }
}
