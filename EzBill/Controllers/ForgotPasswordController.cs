using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Account;
using EzBill.Application.ServiceModel.ForgotPassword;
using EzBill.Models.Request.ForgotPassword;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBill.Controllers
{
	[Route("api/")]
	[ApiController]
	public class ForgotPasswordController : ControllerBase
	{
		private readonly IForgotPasswordService _forgotPasswordService;
		private readonly IAccountService _accountService;

		public ForgotPasswordController(IForgotPasswordService forgotPassword, IAccountService accountService)
		{
			_forgotPasswordService = forgotPassword;
			_accountService = accountService;
		}

		[HttpPost("forgot-password/send")]
		public async Task<IActionResult> SendEmailForgotPassword([FromBody] SendOTPRequest request)
		{
			if (!ModelState.IsValid)
			{
				var error = ModelState.Values
							.SelectMany(v => v.Errors)
							.Select(e => e.ErrorMessage)
							.FirstOrDefault();

				return BadRequest(new
				{
					message = error
				});
			}
			var createOtpModel = new CreateForgotPasswordModel
			{
				Email = request.Email
			};
			var result = await _forgotPasswordService.SaveOTP(createOtpModel);
			if (result)
			{
				return Ok(new { message = "Đã gửi email thành công" });
			}
			else
			{
				return BadRequest(new { message = "Gửi email thất bại" });
			}
		}
		[HttpPost("forgot-password/check-otp")]
		public async Task<IActionResult> CheckOTP([FromBody] CheckOTPRequest request)
		{
			if (!ModelState.IsValid)
			{
				var error = ModelState.Values
							.SelectMany(v => v.Errors)
							.Select(e => e.ErrorMessage)
							.FirstOrDefault();

				return BadRequest(new
				{
					message = error
				});
			}

			var result = await _forgotPasswordService.CheckOTP(request.Email, request.OTP);
			if (result)
			{
				return Ok(new { message = "Xác thực OTP thành công" });
			}
			else
			{
				return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn" });
			}
		}
		[HttpPost("forgot-password/re-pass")]
		public async Task<IActionResult> RePassword([FromBody] RePasswordRequest request)
		{
			var model = new RePasswordModel
			{
				Email = request.Email,
				ConfirmPassword = request.ConfirmNewPassword,
				Password = request.NewPassword
			};
			var result = await _accountService.RePassword(model);
			if (result)
			{
				return Ok(new { message = "Đổi mật khẩu thành công" });
			}
			else
			{
				return BadRequest(new { message = "Đổi mật khẩu thất bại" });
			}
		}
	}
}
