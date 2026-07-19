using CineCompare.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CineCompare.Web.Controllers
{
    // Soruyu almak için küçük bir yardımcı sınıf açıyoruz
    public class ChatbotRequestDto
    {
        public string Question { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece Token'ı olanlar girebilir
    public class ChatbotController : ControllerBase
    {
        private readonly IAiService _aiService;

        public ChatbotController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskBot([FromBody] ChatbotRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Lütfen Sine-Bot'a bir soru sorun.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            var answer = await _aiService.GetMovieRecommendationAsync(request.Question, userId);

            // C# bazen isimsiz tipleri (anonymous types) okurken sorun çıkarabilir, bu format her zaman %100 çalışır:
            return Ok(answer);
        }
    }
}