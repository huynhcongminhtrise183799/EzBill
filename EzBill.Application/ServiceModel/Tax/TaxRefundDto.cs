using System;
using System.Collections.Generic;

namespace EzBill.Application.DTO.TaxRefund
{
    public class TaxRefundRequestDto
    {
        public Guid TripId { get; set; }
        public List<TaxRefundEventRequestDto> Events { get; set; } = new();
    }

    public class TaxRefundEventRequestDto
    {
        public Guid EventId { get; set; }
        public Guid RefundedBy { get; set; }  
        public double RefundPercent { get; set; }  
        public string SplitType { get; set; } 
        public List<TaxRefundBeneficiaryRequestDto> Beneficiaries { get; set; } = new();
    }

    public class TaxRefundBeneficiaryRequestDto
    {
        public Guid AccountId { get; set; }
        public double? Ratio { get; set; }
    }

    public class TaxRefundResponseDto
    {
        public string Message { get; set; }
        public string ProductName { get; set; }
        public string OriginalAmount { get; set; }
        public string RefundPercent { get; set; }
        public string RefundAmount { get; set; }
        public string SplitType { get; set; }
        public List<TaxRefundBeneficiaryResponseDto> Beneficiaries { get; set; }
        public List<TaxRefundEventResponseDto> Events { get; set; }
    }

    public class TaxRefundBeneficiaryResponseDto
    {
        public Guid AccountId { get; set; }
        public string Ratio { get; set; }
        public string AmountReceived { get; set; }
    }

    public class TaxRefundEventResponseDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string OriginalAmount { get; set; }
        public string RefundAmount { get; set; }
    }
}
