using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public enum PlanBillingCycle
	{
		MONTHLY, YEARLY
	}
	public enum PlanStatus
	{
		ACTIVE, INACTIVE
	}
	public enum PlanType
	{
		BASIC, VIP, DAILY
	}
	public class Plan
	{
		public Guid PlanId { get; set; }

		public string PlanName { get; set; }

		public string Description { get; set; }

		public double Price { get; set; }

		public int MaxMembersPerTrip { get; set; }

		public int MaxGroups { get; set; } 

		public string BillingCycle { get; set; } // MONTHLY, YEARLY

		public string Type { get; set; } // BASIC, VIP, DAILY

		public string Status { get; set; } // ACTIVE, INACTIVE

		public virtual ICollection<AccountSubscriptions> AccountSubscriptions { get; set; }
		public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
	}
}
