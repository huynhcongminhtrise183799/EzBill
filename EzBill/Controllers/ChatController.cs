using EzBill.Application.IService;
using EzBill.Application.Service;
using EzBill.Application.ServiceModel.Chat;
using EzBill.Models.Request.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBill.Controllers
{
	[Route("api/chats/")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatService _chatService;

		public ChatController(IChatService chatService)
		{
			_chatService = chatService;
		}
		[HttpPost("send")]
		public async Task<IActionResult> SendMessage([FromBody] SendChatRequest request)
		{
			var model = new ChatModel
			{
				TripId = request.TripId,
				SenderId = request.SenderId,
				Content = request.Content,
				Type = request.Type,
				FileUrl = request.FileUrl,
				SentAt = DateTime.UtcNow
			};
			var message = await _chatService.SendMessageAsync(model);
			return Ok(message);
		}
		[HttpGet("{tripId}")]
		public async Task<IActionResult> GetMessagesByTrip([FromRoute] Guid tripId, [FromQuery] int page = 1)
		{
			var messages = await _chatService.GetMessagesByTripAsync(tripId, page);
			return Ok(messages);
		}
	}
}
