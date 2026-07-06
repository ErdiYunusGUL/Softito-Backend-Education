using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SneakerDropPlatform.Repositories;

namespace SneakerDropPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsRepository _repository;

        public ReportsController(ReportsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            // In a real app, we would validate the admin token here via an Authorize attribute or manual header check.
            var header = Request.Headers["Authorization"].ToString();
            if (header != "Bearer hype_admin_secret_token_2026")
            {
                return Unauthorized();
            }

            var stats = await _repository.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}
