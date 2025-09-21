using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Payment
{
	public class CreatePaymentHistoryModel
	{
		public Guid PaymentHistoryModelId { get; set; }
		public Guid FromAccountId { get; set; }

		public double Amount { get; set; }

		public long OrderCode { get; set; }

		public Guid PlanId { get; set; }

	}
}
