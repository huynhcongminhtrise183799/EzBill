using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Trip
{
    public class TripMemberDto
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
