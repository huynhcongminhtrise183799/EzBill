using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Event
{
    public class CreateEventRequest
    {
        public Guid TripId { get; set; }
        public string EventName { get; set; }
        public string? EventDescription { get; set; }
        public DateOnly EventDate { get; set; }
        public string? ReceiptUrl { get; set; }
        public Guid? PaidBy { get; set; }
        public string Currency { get; set; }
        public double AmountOriginal { get; set; }
        public double? ExchangeRate { get; set; }
        public SplitType SplitType { get; set; }
        public bool IsGroupMoney { get; set; }
        public List<EventUseDto>? EventUses { get; set; } 
    }

    public class EventUseDto
    {
        public Guid AccountId { get; set; }
        public double Ratio { get; set; } 
    }
}
