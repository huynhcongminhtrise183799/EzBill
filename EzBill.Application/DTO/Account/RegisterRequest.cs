using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.DTO.Account
{
	public class RegisterRequest
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string RePassword { get; set; }

		public string PhoneNumber { get; set; }

		public int Gender { get; set; }

		public string NickName { get; set; }

	}
}
