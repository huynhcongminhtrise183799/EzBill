using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class Event
    {
        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public string EventDescription { get; set; }

        public string? ReceiptUrl { get; set; }

        public Guid TripId { get; set; }

        public Guid PaidBy { get; set; }

        public char Currency { get; set; }

        public double AmountOriginal { get; set; }

        public double? ExchangeRate { get; set; }
        public double AmountInTripCurrency { get; set; }

        public Account Account { get; set; }

        public Trip Trip { get; set; }

        public virtual ICollection<Event_Use> Event_Use { get; set; }
        public virtual ICollection<TaxRefund> TaxRefunds { get; set; }

    }
}
