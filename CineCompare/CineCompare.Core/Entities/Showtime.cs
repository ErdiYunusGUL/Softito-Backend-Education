using System;

namespace CineCompare.Core.Entities
{
    public class Showtime : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public string LanguageOption { get; set; }

        // Yeni Eklenen Alan: O spesifik seansın formatı
        public string ScreeningFormat { get; set; } // Örn: 2D, 3D, IMAX 3D, MP4

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int HallId { get; set; }
        public Hall Hall { get; set; }


        public ICollection<TicketPrice> TicketPrices { get; set; }
    }
}