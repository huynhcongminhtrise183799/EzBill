using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Settlement
{
    public class SettlementResultDto
    {
        public Guid SettlementId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string TripName { get; set; }
    }
}
