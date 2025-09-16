﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.Entity
{
    public class ForgotPassword
    {
		public Guid ForgotPasswordId { get; set; }

		public Guid AccountId { get; set; }

		public string OTP { get; set; }

		public DateTime ExpireAt { get; set; }

		public Account Account { get; set; }
	}
}
