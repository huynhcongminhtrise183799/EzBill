using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class Event_Use
    {
        public Guid EventId { get; set; }

        public Guid AccountId { get; set; }
       
        public double? Amount { get; set; }

        public Event Event  { get; set; }

        public Account Account { get; set; }
    }
}
