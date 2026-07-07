namespace SneakerDropPlatform.Models
{
    public class Sneaker
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
