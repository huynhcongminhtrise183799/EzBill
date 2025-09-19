using EzBill.Application.IService;
using Microsoft.AspNetCore.SignalR;

namespace EzBill.ChatHubs
{
	public class ChatHub : Hub
	{
		public async Task JoinGroup(string tripId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, tripId);
		}

		public async Task LeaveGroup(string tripId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, tripId);
		}
	}
}
