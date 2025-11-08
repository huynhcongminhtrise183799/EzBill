using EzBill.Application.IService;
using Microsoft.AspNetCore.SignalR;

namespace EzBill.ChatHubs
{
	public class PaymentSignalRNotifier : IPaymentNotifier
	{
		private readonly IHubContext<PaymentHub> _hubContext;
		public PaymentSignalRNotifier(IHubContext<PaymentHub> hubContext)
		{
			_hubContext = hubContext;
		}
		private const string AdminGroup = "Admins";

		// Tên sự kiện "ping" mà client sẽ lắng nghe
		private const string EventName = "PaymentsUpdated";

		public async Task NotifyNewPaymentAsync()
		{
			await _hubContext.Clients.Group(AdminGroup)
							.SendAsync(EventName);
		}
	}
}
