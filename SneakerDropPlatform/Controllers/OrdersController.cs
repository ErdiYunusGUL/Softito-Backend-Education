using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakerDropPlatform.Models;
using SneakerDropPlatform.Repositories;

namespace SneakerDropPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly SneakerRepository _repository;

        public OrdersController(SneakerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Name and Email are required.");
            }

            if (request.Items == null || !request.Items.Any())
            {
                return BadRequest("Cart is empty.");
            }

            // Calculate total based on passed in prices for simplicity (in a real app, verify prices against DB)
            decimal totalAmount = request.Items.Sum(i => i.Price * i.Quantity);

            bool success = await _repository.CreateOrderAsync(request, totalAmount);

            if (success)
            {
                return Ok(new { message = "Order successfully placed!", totalAmount });
            }

            return StatusCode(500, "An error occurred while processing your order or stock is insufficient.");
        }
    }
}
