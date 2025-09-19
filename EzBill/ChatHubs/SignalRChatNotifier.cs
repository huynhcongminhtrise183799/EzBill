using EzBill.Application.IService;
using Microsoft.AspNetCore.SignalR;

namespace EzBill.ChatHubs
{
	public class SignalRChatNotifier : IChatNotifier
	{
		private readonly IHubContext<ChatHub> _hubContext;

		public SignalRChatNotifier(IHubContext<ChatHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task NotifyMessageAsync(Guid tripId, object messageDto)
		{
			await _hubContext.Clients.Group(tripId.ToString())
				.SendAsync("ReceiveMessage", messageDto);
		}
	}
}
