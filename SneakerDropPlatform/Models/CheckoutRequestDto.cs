using System.Collections.Generic;

namespace SneakerDropPlatform.Models
{
    public class CheckoutRequestDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }

    public class CartItemDto
    {
        public int DropId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
