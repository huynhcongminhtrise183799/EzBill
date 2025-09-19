using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Chat
{
	public  class GetMessageModel
	{
		public Guid MessageId { get; set; }
		public Guid TripId { get; set; }
		public Guid SenderId { get; set; }
		public string NickName { get; set; }
		public string? AvatarUrl { get; set; }
		public string Content { get; set; }
		public string MessageType { get; set; }
		public string? FileUrl { get; set; }
		public DateTime SentAt { get; set; }
	}
}
