using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Chat
{
    public class ChatModel
    {
		public Guid TripId { get; set; }
		public Guid SenderId { get; set; } 
		public string Content { get; set; }
		public string Type { get; set; }  // "text" | "image" | "file" | "system"

		public string? FileUrl { get; set; }

		public DateTime SentAt { get; set; }

	}
}
