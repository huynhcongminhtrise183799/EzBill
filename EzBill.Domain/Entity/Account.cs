using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Trip> Trip { get; set; }

        public virtual ICollection<TripMember> TripMembers { get; set; }

        public virtual ICollection<Event_Use> Event_Use { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public ICollection<Settlement> SettlementsFrom { get; set; }


        public ICollection<Settlement> SettlementsTo { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoriesFrom { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistoriesTo { get; set; }
        public virtual ICollection<TaxRefund> TaxRefunds { get; set; }
        public virtual ICollection<TaxRefund_Usage> TaxRefund_Usages { get; set; }

    }
}
