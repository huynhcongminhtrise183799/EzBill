using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
	public class TaxRefund_Event
	{
		public Guid TaxRefundId { get; set; }
		public Guid EventId { get; set; }

		public TaxRefund TaxRefund { get; set; }
		public Event Event { get; set; }
	}
}
