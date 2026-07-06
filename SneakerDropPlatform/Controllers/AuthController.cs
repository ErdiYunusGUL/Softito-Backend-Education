using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakerDropPlatform.Models;
using SneakerDropPlatform.Repositories;

namespace SneakerDropPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _repository;

        public AuthController(AuthRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and Password are required.");

            bool isValid = await _repository.ValidateAdminAsync(request.Username, request.Password);

            if (isValid)
            {
                // Simple token for demo. In a real app, generate a JWT.
                string token = "hype_admin_secret_token_2026";
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials.");
        }
    }
}
