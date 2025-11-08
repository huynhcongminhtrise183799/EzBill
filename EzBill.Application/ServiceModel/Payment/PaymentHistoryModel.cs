using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Payment
{
	public class PaymentHistoryModel
	{
		public Guid PaymentHistoryModelId { get; set; }
		public Guid UserId { get; set; }

		public Guid PlanId { get; set; }
		public string Email { get; set; } = null!;

		public string PlanName { get; set; } = null!;

		public double Amount { get; set; }

		public DateOnly PaymentDate { get; set; }

		public string Status { get; set; } = null!;

	}
}
