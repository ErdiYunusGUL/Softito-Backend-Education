namespace CineCompare.Core.Entities
{
    public class TicketPrice : BaseEntity
    {
        public decimal Price { get; set; }
        public string TicketCategory { get; set; } // Örn: Adult (Tam), Student (Öğrenci)

        // Foreign Key: Bu fiyat hangi seansa ait?
        public int ShowtimeId { get; set; }
        public Showtime Showtime { get; set; }
    }
}