using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO
{
    public class CreateEventDTO
    {
        public string EventName { get; set; }

        public string EventDescription { get; set; }

        public string? ReceiptUrl { get; set; }

        public Guid TripId { get; set; }

        public Guid PaidBy { get; set; }

        public string Currency { get; set; }

        public double AmountOriginal { get; set; }

        public double? ExchangeRate { get; set; }
        public double AmountInTripCurrency { get; set; }

        public List<Event_Used_DTO> Event_Used { get; set; }
    }

    public class Event_Used_DTO
    {
        public Guid AccountId { get; set; }
    }
}
