using System;

namespace CineCompare.Core.DTOs
{
    public class ShowtimePricingDto
    {
        public string TheaterName { get; set; }
        public string HardwareType { get; set; } // Örn: IMAX, VIP
        public DateTime StartTime { get; set; }

        // Fiyatları net bir şekilde ayırdık
        public decimal? StudentPrice { get; set; }
        public decimal? AdultPrice { get; set; }
    }
}