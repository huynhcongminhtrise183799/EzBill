using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum SettlementStatus
    {
        PAID, UNPAID
    }
    public class Settlement
    {
        public Guid SettlementId { get; set; }

        public Guid TripId { get; set; }

        public Guid FromAccountId { get; set; }

        public Guid ToAccountId { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public Trip Trip { get; set; }

        public Account FromAccount { get; set; }

        public Account ToAccount { get; set; }
    }
}
