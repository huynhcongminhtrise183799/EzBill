using EzBill.Domain.Entity;

namespace EzBill.Models.Request.Chat
{
	public class SendChatRequest
	{
		public Guid TripId { get; set; }
		public Guid SenderId { get; set; }
		public string Content { get; set; }
		public string Type { get; set; }  // "text" | "image" | "file" | "system"
		public string? FileUrl { get; set; }
	}
}
