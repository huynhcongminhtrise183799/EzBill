using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Trip
{
	public class UpdateTripModel
	{
		public string Name { get; set; }

		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }

		public string? AvatarTrip { get; set; }
		public double? Budget { get; set; }

		public bool isDelete { get; set; }
		public List<UpdateTripMemberModel> TripMembers { get; set; }
	}
}
