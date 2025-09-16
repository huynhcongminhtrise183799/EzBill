using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
	public class ForgotPasswordRepository : IForgotPasswordRepository
	{
		private readonly EzBillDbContext _context;

		public ForgotPasswordRepository(EzBillDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Delete(Guid id)
		{
			var fp = await _context.ForgotPasswords.FirstOrDefaultAsync(f => f.ForgotPasswordId == id);
			
			if(fp != null)
			{
				 _context.Remove(fp);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<ForgotPassword?> GetOTPByAccountId(Guid accountId)
		{ 
			return await _context.ForgotPasswords.Include(f => f.Account).FirstOrDefaultAsync(fp => fp.AccountId == accountId);
		}

		public async Task<bool> SaveOTP(ForgotPassword forgotPassword)
		{
			try
			{
				await _context.ForgotPasswords.AddAsync(forgotPassword);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}
	}
}
