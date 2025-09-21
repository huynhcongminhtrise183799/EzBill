using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.AccountSubscriptions
{
	public class CreateAccountSubscriptionsModel
	{
		public Guid AccountId { get; set; }

		public Guid PlanId { get; set; }
	}
}
