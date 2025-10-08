using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Trip
{
    public class TripDto
    {
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double? Budget { get; set; }
		public string? AvatarTrip { get; set; }
		public List<TripMemberDto> Members { get; set; }
    }
}
