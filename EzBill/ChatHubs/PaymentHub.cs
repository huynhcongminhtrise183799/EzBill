using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace EzBill.ChatHubs
{
	public class PaymentHub : Hub
	{
		// Tên nhóm cố định cho tất cả admin
		private const string AdminGroup = "Admins";

		// Client (Admin) sẽ gọi hàm này sau khi kết nối
		public async Task JoinAdminGroup()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroup);
		}

		// (Tùy chọn) Client gọi khi rời đi
		public async Task LeaveAdminGroup()
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, AdminGroup);
		}
	}
}
