using EzBill.Application.ServiceModel.Trip;

namespace EzBill.Models.Request.Trip
{
    public class UpdateTripRequest
    {
		public string Name { get; set; }

		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }

		public string? AvatarTrip { get; set; }
		public double? AddMoreBudgetInTrip { get; set; }

		public bool isDelete { get; set; }
		public List<UpdateTripMemberRequest> TripMembers { get; set; }
	}
	public class UpdateTripMemberRequest
	{
		public Guid AccountId { get; set; }
		public Guid TripId { get; set; }
		public double? Amount { get; set; }
	}
}
