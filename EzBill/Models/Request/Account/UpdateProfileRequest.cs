using System.ComponentModel.DataAnnotations;

namespace EzBill.Models.Request.Account
{
	public class UpdateProfileRequest
	{
		public Guid AccountId { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập email")]
		[EmailAddress(ErrorMessage = "Không đúng định dạng")]
		public string Email { get; set; }

		[RegularExpression(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$",
		ErrorMessage = "Số điện thoại không hợp lệ")]
		public string PhoneNumber { get; set; }

		public string? AvatarUrl { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập NickName")]
		public string NickName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập giới tính")]
		public string Gender { get; set; }
	}
}
