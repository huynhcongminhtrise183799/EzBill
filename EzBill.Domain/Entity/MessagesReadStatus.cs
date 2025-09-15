using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public class MessagesReadStatus
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }   // MongoDB tự tạo Id cho document này

		[BsonElement("messageId")]
		public Guid MessageId { get; set; }   // Tham chiếu Messages.MessagesId (GUID)

		[BsonElement("accountId")]
		public Guid AccountId { get; set; }    // Tham chiếu Postgres Account.AccountId

		[BsonElement("isRead")]
		public bool IsRead { get; set; } = false;

		[BsonElement("readAt")]
		public DateTime? ReadAt { get; set; } // null nếu chưa đọ
	}
}
