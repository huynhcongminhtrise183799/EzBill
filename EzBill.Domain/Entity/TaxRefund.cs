using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    //public enum TaxRefundStatus
    //{
    //    PENDING, SUCCCESS, REJECT
    //}
    public enum TaxRefundSplitType
    {
        KEEP, EQUAL, RATIO
    }
    public class TaxRefund
    {
        public Guid TaxRefundId { get; set; }

        //public Guid EventId { get; set; }

        public Guid TripId { get; set; }

        public string ProductName { get; set; }

        public double OriginalAmount { get; set; }

        [Range(0, 100)]
        public double RefundPercent { get; set; }

        public double RefundAmount { get; set; }

        public Guid RefundedBy { get; set; }

        public bool IsGroupMoneyUsed { get; set; }

        public TaxRefundSplitType SplitType { get; set; }

        //public TaxRefundStatus Status { get; set; } = TaxRefundStatus.PENDING;

        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //public Event Event { get; set; }

        public Trip Trip { get; set; }

        public Account Account { get; set; }

        public virtual ICollection<TaxRefund_Usage> TaxRefund_Usages { get; set; }
        
    }
}
