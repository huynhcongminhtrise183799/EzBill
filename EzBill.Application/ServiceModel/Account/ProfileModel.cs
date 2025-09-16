﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.ServiceModel.Account
{
	public class ProfileModel
	{
		public Guid AccountId { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string? AvatarUrl { get; set; }

		public string NickName { get; set; }

		public string Gender { get; set; }

		public string Role { get; set; }
	}
}
