using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request
{
    public class CreateTripDTO
    {
        public string TripName { get; set; }
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public double? Budget { get; set; }

        public List<TripMemberDTO> TripMember { get; set; }
    }

    public class TripMemberDTO
    {
        public string AccountId { get; set; }

        public double? Amount { get; set; }
    }
}
