using System;
using System.Collections.Generic;

namespace EzBill.Application.DTO.TaxRefund
{
    public class TaxRefundDto
    {
        public Guid TaxRefundId { get; set; }
        public string ProductName { get; set; }
        public double OriginalAmount { get; set; }
        public double RefundPercent { get; set; }
        public double RefundAmount { get; set; }
        public Guid RefundedBy { get; set; }
        public string SplitType { get; set; }
        public List<RefundBeneficiaryDto> Beneficiaries { get; set; }
    }

    public class RefundBeneficiaryDto
    {
        public Guid AccountId { get; set; }
        public double Ratio { get; set; }
        public double AmountReceived { get; set; }
    }
}
