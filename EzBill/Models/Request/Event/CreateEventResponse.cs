using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request.Event
{
    public class CreateEventResponse
    {
        public Guid EventId { get; set; }
        public string Message { get; set; }
        public double TotalAmount { get; set; }
        public List<BeneficiaryDto> Beneficiaries { get; set; }
    }

    public class BeneficiaryDto
    {
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
