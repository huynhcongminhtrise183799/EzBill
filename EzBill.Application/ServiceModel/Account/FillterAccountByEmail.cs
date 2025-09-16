using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Account
{
    public class FillterAccountByEmail
    {
        public Guid AccountId { get; set; }

		public string Email { get; set; }

		public string NickName { get; set; } 

		public string? Avatar { get; set; } 
	}
}
