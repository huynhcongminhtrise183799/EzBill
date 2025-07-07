using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum TaxRefundSplitType
    {
        KEEP, EQUAL, RATIO
    }
    public class TaxRefund
    {
        public Guid TaxRefundId { get; set; }

        public Guid EventId { get; set; }
        
        public string ProductName { get; set; }

        public double OriginalAmount { get; set; }

        public double RefundPercent { get; set; }

        public double RefundAmount { get; set; }

        public Guid RefundedBy { get; set; }

        public bool IsGroupMoneyUsed { get; set; }

        public string SplitType { get; set; }

        public Event Event { get; set; }

        public Account Account { get; set; }

        public virtual ICollection<TaxRefund_Usage> TaxRefund_Usages { get; set; }
        
    }
}
