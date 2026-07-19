using CineCompare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CineCompare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        public class ChatRequest
        {
            public string Prompt { get; set; }
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request?.Prompt))
            {
                return BadRequest("Lütfen bir mesaj girin.");
            }

            var userId = "test_user_id"; 
            var answer = await _aiService.GetMovieRecommendationAsync(request.Prompt, userId);
            
            return Ok(new { Answer = answer });
        }
    }
}
