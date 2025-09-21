using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface IUserDeviceTokenService
    {
		Task<bool> SaveDeviceToken(Guid accountId, string deviceToken, string fcmToken);
		Task<List<string>> GetTokensByAccountIdsAsync(IEnumerable<Guid> accountIds);
		Task<List<string>> GetDeviceTokensByAccountId(Guid accountId);


	}
}
