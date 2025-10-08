using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Trip
{
    public class UpdateTripMemberModel
    {
		public Guid AccountId { get; set; }
		public Guid TripId { get; set; }
		public double? Amount { get; set; }
	}
}
