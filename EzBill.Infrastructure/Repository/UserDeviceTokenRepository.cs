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
	public class UserDeviceTokenRepository : IUserDeviceTokenRepository
	{
		private readonly EzBillDbContext _context;

		public UserDeviceTokenRepository(EzBillDbContext context)
		{
			_context = context;
		}

		public async Task<bool> AddODeviceToken(UserDeviceToken user)
		{
			try
			{
				await _context.UserDeviceTokens.AddAsync(user);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<UserDeviceToken?> GetDeviceTokenByFCMAndDeviceId(string FCMToken, string deviceId)
		{
			return await _context.UserDeviceTokens
				.FirstOrDefaultAsync(udt => udt.FCMToken == FCMToken && udt.DeviceId == deviceId);
		}

		public async Task<List<string>> GetDeviceTokensByAccountId(Guid accountId)
		{
			return await _context.UserDeviceTokens
			.Where(t => t.AccountId == accountId)
			.Select(t => t.FCMToken)
			.Distinct()
			.ToListAsync();
		}

		public async Task<List<string>> GetTokensByAccountIdsAsync(IEnumerable<Guid> accountIds)
		{
			return await _context.UserDeviceTokens
			.Where(t => accountIds.Contains(t.AccountId))
			.Select(t => t.FCMToken)
			.Distinct()
			.ToListAsync();
		}

		public Task<bool> RemoveDeviceToken(Guid accountId, string deviceToken)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateODeviceToken(UserDeviceToken user)
		{
			var existingToken = _context.UserDeviceTokens
				.FirstOrDefault(udt => udt.DeviceId == user.DeviceId);
			if (existingToken != null)
			{
				existingToken.AccountId = user.AccountId;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}
	}
}
