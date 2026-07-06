using System;

namespace SneakerDropPlatform.Models
{
    public class ActiveDropDto
    {
        public int DropId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DropDate { get; set; }
        public int TotalStock { get; set; }
    }
}
