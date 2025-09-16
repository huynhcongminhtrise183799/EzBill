using EzBill.Application.DTO.TaxRefund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request.Trip
{
    public class TripDetailsDto
    {
        public string TripName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double? Budget { get; set; }

        public double TotalEventAmount { get; set; }       
        public double TotalTaxRefundAmount { get; set; }   
        public double TotalUsedAmount { get; set; }

        public List<EventContributionDto> EventContributions { get; set; } 
        public List<TaxRefundDto> TaxRefunds { get; set; }
        public List<TripMemberDto> Members { get; set; }
    }
    public class EventContributionDto
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public double PaidAmount { get; set; }
    }
}
