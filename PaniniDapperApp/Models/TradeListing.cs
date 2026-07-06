namespace PaniniDapperApp.Models
{
    public class TradeListing
    {
        public int Id { get; set; }
        public int CollectorId { get; set; }
        public int StickerId { get; set; }
        public string Status { get; set; } = "Open"; // Open, Completed, Cancelled
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Collector? Collector { get; set; }
        public Sticker? Sticker { get; set; }
    }
}
