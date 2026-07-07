using System;

namespace ScoutingAPI.Entities
{
    public class ScoutReport
    {
        public int Id { get; set; }
        
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        
        public int ScoutId { get; set; }
        public Scout Scout { get; set; } = null!;
        
        public int Rating { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
    }
}
