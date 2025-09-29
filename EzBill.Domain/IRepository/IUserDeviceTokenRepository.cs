using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IUserDeviceTokenRepository
	{
		Task<bool> AddODeviceToken(UserDeviceToken user);
		Task<bool> UpdateODeviceToken(UserDeviceToken user);

		Task<bool> RemoveDeviceToken(Guid accountId, string deviceToken);
		Task<List<string>> GetDeviceTokensByAccountId(Guid accountId);
		Task<List<string>> GetTokensByAccountIdsAsync(IEnumerable<Guid> accountIds);

		Task<UserDeviceToken?> GetDeviceTokenByFCMAndDeviceId(string FCMToken);
	}
}
