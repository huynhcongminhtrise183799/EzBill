using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request.Settlement
{
    public class GenerateSettlementRequest
    {
        public Guid TripId { get; set; }
    }
}
