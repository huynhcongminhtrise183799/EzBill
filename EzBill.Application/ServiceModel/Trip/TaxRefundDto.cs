using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Trip
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
    }
    public class TaxRefundBeneficiaryDto
    {
        public Guid AccountId { get; set; }
        public string? Avatar { get; set; }

		public string? NickName { get; set; }
		public string Ratio { get; set; }         
        public string AmountReceived { get; set; } 
    }
}
