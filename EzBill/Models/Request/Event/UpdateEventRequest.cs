using EzBill.Domain.Entity;

namespace EzBill.Models.Request.Event
{
	public class UpdateEventRequest
	{
		public Guid TripId { get; set; }
		public string EventName { get; set; }
		public string? EventDescription { get; set; }
		public DateOnly EventDate { get; set; }
		public string? ReceiptUrl { get; set; }
		public Guid PaidBy { get; set; }
		public string Currency { get; set; }
		public double AmountOriginal { get; set; }
		public double? ExchangeRate { get; set; }
		public SplitType SplitType { get; set; }
		public bool IsGroupMoney { get; set; }
		public List<UpdateEventUseDto>? EventUses { get; set; }
	}
	public class UpdateEventUseDto
	{
		public Guid AccountId { get; set; }
		public double Ratio { get; set; }
	}
}
