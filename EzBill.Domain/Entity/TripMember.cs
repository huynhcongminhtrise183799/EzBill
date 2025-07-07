using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public enum TripMemberStatus
    {
        ACTIVE,INACTIVE
    }
    public class TripMember
    {
        public Guid TripId { get; set; }

        public Guid AccountId { get; set; }

        public double? Amount { get; set; }

        public string Status { get; set; }

        public Trip Trip { get; set; }

        public Account Account { get; set; }
    }
}
