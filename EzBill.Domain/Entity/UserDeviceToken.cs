using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public class UserDeviceToken
	{
		public Guid Id { get; set; }

		public Guid AccountId { get; set; }

		public string FCMToken { get; set; } 

		public Account Account { get; set; } 
	}
}
