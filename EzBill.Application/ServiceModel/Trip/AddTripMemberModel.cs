using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Trip
{
	public class AddTripMemberModel
	{
		public Guid AccountId { get; set; }
		public Guid TripId { get; set; }
	}
}
