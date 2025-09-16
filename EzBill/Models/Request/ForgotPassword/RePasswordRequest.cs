using System.ComponentModel.DataAnnotations;

namespace EzBill.Models.Request.ForgotPassword
{
	public class RePasswordRequest
	{
		[Required(ErrorMessage = "Nhập email")]
		[EmailAddress(ErrorMessage = "Email không đúng định dạng")]
		public string Email { get; set;}

		[Required(ErrorMessage = "Nhập password mới")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Xác nhận password mới")]
		public string ConfirmNewPassword { get; set; }
	}
}
