using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Models.Request.Account
{
	public class RegisterRequest
	{
		[EmailAddress(ErrorMessage = "Email không đúng định dạng")]
		public string Email { get; set; }
		[MinLength(6, ErrorMessage = "Mật khẩu phải lớn hơn 6 ký tự")]
		public string Password { get; set; }

		public string RePassword { get; set; }

		[RegularExpression(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$",
		ErrorMessage = "Số điện thoại không hợp lệ")]
		public string PhoneNumber { get; set; }

		public int Gender { get; set; }

		public string NickName { get; set; }

	}
}
