using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakerDropPlatform.Repositories;

namespace SneakerDropPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DropsController : ControllerBase
    {
        private readonly SneakerRepository _repository;

        public DropsController(SneakerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveDrops()
        {
            var drops = await _repository.GetActiveDropsAsync();
            return Ok(drops);
        }

        public class WaitlistRequest
        {
            public string Email { get; set; } = string.Empty;
        }

        [HttpPost("{id}/waitlist")]
        public async Task<IActionResult> JoinWaitlist(int id, [FromBody] WaitlistRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Email is required.");
            }

            bool success = await _repository.JoinWaitlistAsync(id, request.Email);
            if (success)
            {
                return Ok(new { message = "Successfully joined the waitlist!" });
            }

            return StatusCode(500, "An error occurred while joining the waitlist.");
        }
    }
}
