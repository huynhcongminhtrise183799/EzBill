using System;
using System.Collections.Generic;

namespace EzBill.Application.DTO.Event
{
    public class EventDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateOnly EventDate { get; set; }
        public Guid? PaidBy { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? AvartarPaidBy { get; set; }
		public string? NickNamePaidBy { get; set; }
		public double AmountInTripCurrency { get; set; }
        public List<BeneficiaryDto> Beneficiaries { get; set; }
    }

}
