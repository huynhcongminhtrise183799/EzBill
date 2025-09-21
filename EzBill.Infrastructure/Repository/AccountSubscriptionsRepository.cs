using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
	public class AccountSubscriptionsRepository : IAccountSubscriptionsRepository
	{
		private readonly EzBillDbContext _context;
		public AccountSubscriptionsRepository(EzBillDbContext context)
		{
			_context = context;
		}
		public async Task<bool> AddAccountSubscriptions(AccountSubscriptions accountSubscriptions)
		{
			try
			{
				await _context.AccountSubscriptions.AddAsync(accountSubscriptions);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
