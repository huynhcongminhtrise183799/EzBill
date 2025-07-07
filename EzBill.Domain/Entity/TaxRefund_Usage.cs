using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class TaxRefund_Usage
    {
        public Guid TaxRefundId { get; set; }

        public Guid AccountId { get; set; }

        public double? Ratio { get; set; }

        public double AmountReceived { get; set; }

        public TaxRefund TaxRefund { get; set; }

        public Account Account { get; set; }
    }
}
