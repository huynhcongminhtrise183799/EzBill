using System.ComponentModel.DataAnnotations;

namespace EzBill.Models.Request.ForgotPassword
{
	public class CheckOTPRequest
	{
		[Required(ErrorMessage = "Vui lòng nhập email")]
		[EmailAddress(ErrorMessage = "Không đúng định dạng")]
		public string Email { get; set; }

		[MinLength(6, ErrorMessage = "OTP là 6 ký tự")]
		[MaxLength(6, ErrorMessage = "OTP là 6 ký tự")]
		public string OTP { get; set; }
	}
}
