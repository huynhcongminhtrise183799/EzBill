using EzBill.Application.ServiceModel.ForgotPassword;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IForgotPasswordService
	{
		Task<bool> SaveOTP(CreateForgotPasswordModel forgotPassword);

		Task<OTPResponse?> GetOTPByAccountId(Guid accountId);

		Task<bool> CheckOTP(string email, string otp);
	}
}
