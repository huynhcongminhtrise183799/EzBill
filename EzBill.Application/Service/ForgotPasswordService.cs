using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.ForgotPassword;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class ForgotPasswordService : IForgotPasswordService
	{
		private readonly IForgotPasswordRepository _repo;
		private readonly IEmailService _emailService;
		private readonly IAccountService _accountService;
		public ForgotPasswordService(IForgotPasswordRepository repo, IEmailService emailService, IAccountService accountService)
		{
			_repo = repo;
			_emailService = emailService;
			_accountService = accountService;
		}

		public async Task<bool> CheckOTP(string email, string otp)
		{
			var account = await _accountService.GetAccountByEmail(email);

			var fp = await _repo.GetOTPByAccountId(account.AccountId);
			if (fp == null) throw new AppException("Không có otp dưới db", 404);
			if (fp.OTP != otp) throw new AppException("OTP không đúng", 400);
			if (fp.ExpireAt < DateTime.UtcNow) throw new AppException("OTP đã hết hạn", 400);
			await _repo.Delete(fp.ForgotPasswordId);
			return true;
		}

		public async Task<OTPResponse?> GetOTPByAccountId(Guid accountId)
		{
			var fp =  await _repo.GetOTPByAccountId(accountId);
			if (fp == null) throw new AppException("Không có otp dưới db", 404);
			var response = new OTPResponse
			{
				Email = fp.Account.Email,
				OTP = fp.OTP
			};
			return response;
		}

		public async Task<bool> SaveOTP(CreateForgotPasswordModel forgotPassword)
		{
			var otp = new Random().Next(100000, 999999).ToString();
			string subject = "Your OTP Code";
			string body = $"Mã OTP của bạn là: {otp}";
			await _emailService.SendEmailAsync(forgotPassword.Email, subject, body);
			var account = await _accountService.GetAccountByEmail(forgotPassword.Email);
			if (account == null) throw new AppException("Không tìm thấy tài khoản với email này", 404);
			var checkExist = await _repo.GetOTPByAccountId(account.AccountId);
			if(checkExist != null)
			{
				await _repo.Delete(checkExist.ForgotPasswordId);
			}
			var fp = new ForgotPassword
			{
				ForgotPasswordId = Guid.NewGuid(),
				AccountId = account.AccountId,
				OTP = otp,
				ExpireAt = DateTime.UtcNow.AddMinutes(5)
			};
			return await _repo.SaveOTP(fp);
			
		}
	}
}
