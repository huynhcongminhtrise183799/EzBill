using System;
using System.Collections.Generic;

namespace EzBill.Application.DTO
{
    public class TaxRefundRequestDto
    {
        public string ProductName { get; set; }
        public double OriginalAmount { get; set; }
        public double RefundPercent { get; set; }
        public Guid RefundedBy { get; set; }
        public int SplitType { get; set; }
        public List<TaxRefundUsageDto> TaxRefund_Usages { get; set; }
    }

    public class TaxRefundUsageDto
    {
        public Guid AccountId { get; set; }
        public double? Ratio { get; set; }
    }
}
