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

		public async Task<AccountSubscriptions?> GetByAccountId(Guid accountId)
		{
			return await  _context.AccountSubscriptions.Include(p => p.Plan)
				.FirstOrDefaultAsync(a => a.AccountId == accountId && a.Status == SubscriptionStatus.ACTIVE.ToString());
		}

		public async Task<bool> UpdateSubscriptions(AccountSubscriptions accountSubscriptions)
		{
			var existingSubscription = await _context.AccountSubscriptions
				.FirstOrDefaultAsync(a => a.SubscriptionId == accountSubscriptions.SubscriptionId);
			if (existingSubscription == null) return false;
			existingSubscription.PlanId = accountSubscriptions.PlanId;
			existingSubscription.StartDate = accountSubscriptions.StartDate;
			existingSubscription.EndDate = accountSubscriptions.EndDate;
			existingSubscription.GroupRemaining = accountSubscriptions.GroupRemaining;
			existingSubscription.Status = accountSubscriptions.Status;
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
