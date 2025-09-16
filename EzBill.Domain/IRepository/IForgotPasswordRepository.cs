using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IForgotPasswordRepository
	{
		Task<bool> SaveOTP(ForgotPassword forgotPassword);

		Task<ForgotPassword?> GetOTPByAccountId(Guid accountId);

		Task<bool> Delete(Guid id);

	}
}
