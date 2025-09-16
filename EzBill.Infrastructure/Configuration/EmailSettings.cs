﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Configuration
{
	public class EmailSettings
	{
		public string From { get; set; }
		public string DisplayName { get; set; }
		public string Password { get; set; }
		public string SmtpServer { get; set; }
		public int Port { get; set; }
	}
}
