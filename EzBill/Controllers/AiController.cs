using EzBill.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace Ezbill.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return BadRequest("Message cannot be empty.");

            var response = await _aiService.GetEzbillResponseAsync(userMessage);
            return Ok(new { message = response });
        }
    }
}
