using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Plan
{
	public class PlanModel
	{
		public Guid PlanId { get; set; }

		public string PlanName { get; set; }

		public string Description { get; set; }

		public double Price { get; set; }

		public int MaxMembersPerTrip { get; set; }

		public int MaxGroups { get; set; }

		public string BillingCycle { get; set; } // MONTHLY, YEARLY

		public string Status { get; set; } // ACTIVE, INACTIVE
	}
}
