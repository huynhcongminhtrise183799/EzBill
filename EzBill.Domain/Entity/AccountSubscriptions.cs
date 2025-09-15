using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public enum SubscriptionStatus
	{
		ACTIVE, INACTIVE, CANCELLED
	}
	public class AccountSubscriptions
	{
		public Guid SubscriptionId { get; set; }

		public Guid AccountId { get; set; }

		public Guid PlanId { get; set; }

		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Status { get; set; } // ACTIVE, INACTIVE, CANCELLED

		public Account Account { get; set; }

		public Plan Plan { get; set; }
	}
}
