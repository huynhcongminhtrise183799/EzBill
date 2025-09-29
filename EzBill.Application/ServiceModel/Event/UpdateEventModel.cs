using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Event
{
    public class UpdateEventModel
	{
		public Guid EventId { get; set; }
		public Guid TripId { get; set; }
		public string EventName { get; set; }
		public string? EventDescription { get; set; }
		public DateOnly EventDate { get; set; }
		public string? ReceiptUrl { get; set; }
		public Guid PaidBy { get; set; }
		public string Currency { get; set; }
		public double AmountOriginal { get; set; }
		public double? ExchangeRate { get; set; }
		public bool IsGroupMoney { get; set; }
		public SplitType SplitType { get; set; }
		public List<EUpdateventUseModel>? EventUses { get; set; }
	}

	public class EUpdateventUseModel
	{
		public Guid AccountId { get; set; }
		public double Ratio { get; set; }
	}
}
