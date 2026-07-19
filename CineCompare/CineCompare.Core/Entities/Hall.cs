using System.Collections.Generic;

namespace CineCompare.Core.Entities
{
    public class Hall : BaseEntity
    {
        public string Name { get; set; }
        public string HardwareType { get; set; } // Örn: Standart, VIP, IMAX, 4DX (Fiziksel altyapı)
        public int Capacity { get; set; } // Hazır buradayken Koltuk Kapasitesi de ekleyelim

        public int TheaterId { get; set; }
        public Theater Theater { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }
    }
}