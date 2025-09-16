using System;
using System.Collections.Generic;

namespace EzBill.Models.Request
{
    public class TaxRefundRequestDto
    {
        public Guid TripId { get; set; }
        public string ProductName { get; set; }
        public double OriginalAmount { get; set; }
        public double RefundPercent { get; set; }
        public Guid RefundedBy { get; set; }
        public string SplitType { get; set; }
        public List<TaxRefundUsageDto> TaxRefund_Usages { get; set; }
    }

    public class TaxRefundUsageDto
    {
        public Guid AccountId { get; set; }
        public double? Ratio { get; set; }
    }
}
