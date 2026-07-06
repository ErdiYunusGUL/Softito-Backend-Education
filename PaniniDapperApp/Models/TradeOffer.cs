namespace PaniniDapperApp.Models
{
    public class TradeOffer
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public int OfferedByCollectorId { get; set; }
        public int OfferedStickerId { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public TradeListing? Listing { get; set; }
        public Collector? OfferedBy { get; set; }
        public Sticker? OfferedSticker { get; set; }
    }
}
