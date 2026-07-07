namespace SneakerDropPlatform.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int DropId { get; set; }
        public string Size { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
