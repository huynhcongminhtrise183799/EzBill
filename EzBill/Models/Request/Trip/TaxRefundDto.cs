using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request.Trip
{
    public class TaxRefundDto
    {
        public string Message { get; set; }        
        public string ProductName { get; set; }
        public string OriginalAmount { get; set; }  
        public string RefundPercent { get; set; }   
        public string RefundAmount { get; set; }   
        public string SplitType { get; set; }      
        public List<TaxRefundBeneficiaryDto> Beneficiaries { get; set; }
        public List<TaxRefundEventDto> Events { get; set; }

    }
    public class TaxRefundBeneficiaryDto
    {
        public Guid AccountId { get; set; }
        public string Ratio { get; set; }         
        public string AmountReceived { get; set; } 
    }

    public class TaxRefundEventDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string OriginalAmount { get; set; }
        public string RefundAmount { get; set; }
    }
}
