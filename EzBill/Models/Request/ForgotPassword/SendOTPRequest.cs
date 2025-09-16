using System.ComponentModel.DataAnnotations;

namespace EzBill.Models.Request.ForgotPassword
{
	public class SendOTPRequest
	{
		[Required(ErrorMessage = "Vui lòng nhập email")]
		[EmailAddress(ErrorMessage = "Không đúng định dạng")]
		public string Email { get; set; }
	}
}
