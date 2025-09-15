using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public enum MessageType
	{
		Text,
		Image,
		File,
		System
	}

	public class Messages
	{
		[BsonId]
		public Guid MessageId { get; set; }
		public Guid TripId { get; set; }      
		public Guid SenderId { get; set; }    

		public string Content { get; set; }
		public string MessageType { get; set; }  // "text" | "image" | "file" | "system"
		public string? FileUrl { get; set; }

		public DateTime SendAt { get; set; } = DateTime.UtcNow;

		public bool IsDeleted { get; set; } = false;
	}
}
