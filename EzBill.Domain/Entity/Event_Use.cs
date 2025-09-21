using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class Event_Use
    {
        public Guid EventId { get; set; } // 1 

        public Guid AccountId { get; set; } // 123
       
        public double? AmountFromGroup { get; set; } // 5

        public double? AmountFromPersonal { get; set; } // 20

		public Event Event  { get; set; }

        public Account Account { get; set; }
    }
}
// tao event an toi: