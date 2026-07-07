namespace PaniniDapperApp.Models
{
    public class AlbumItem
    {
        public int Id { get; set; }
        public int CollectorId { get; set; }
        public int StickerId { get; set; }
        public int Quantity { get; set; } = 1;
        
        public Sticker? Sticker { get; set; }
    }
}
