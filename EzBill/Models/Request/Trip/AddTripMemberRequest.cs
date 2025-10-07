namespace EzBill.Models.Request.Trip
{
    public class AddTripMemberRequest
    {
		public Guid AccountId { get; set; }
		public Guid TripId { get; set; }
	}
}
