using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum TripStatus
	{
		ACTIVE, INACTIVE
	}
	public class Trip
    {
        public Guid TripId { get; set; }

        public string TripName { get; set; }

        public Guid CreatedBy { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string? AvatarTrip { get; set; }

		public double? Budget { get; set; }

        public string Status { get; set; } // ACTIVE, INACTIVE

		public Account Account { get; set; }

        public virtual ICollection<TripMember> TripMembers { get; set; }
        public virtual ICollection<Settlement> Settlements { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<TaxRefund> TaxRefunds { get; set; }

    }
}
