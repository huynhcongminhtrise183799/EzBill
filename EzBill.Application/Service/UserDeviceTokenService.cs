using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class UserDeviceTokenService : IUserDeviceTokenService
	{
		private readonly IUserDeviceTokenRepository _userDeviceTokenRepository;
		public UserDeviceTokenService(IUserDeviceTokenRepository userDeviceTokenRepository)
		{
			_userDeviceTokenRepository = userDeviceTokenRepository;
		}

		public async Task<List<string>> GetDeviceTokensByAccountId(Guid accountId)
		{
			return await _userDeviceTokenRepository.GetDeviceTokensByAccountId(accountId);
		}

		public async Task<List<string>> GetTokensByAccountIdsAsync(IEnumerable<Guid> accountIds)
		{
			return await _userDeviceTokenRepository.GetTokensByAccountIdsAsync(accountIds);
		}

		public Task<bool> SaveDeviceToken(Guid accountId, string deviceToken, string fcmToken)
		{
		   var newDeviceToken = new UserDeviceToken
		   {
			   Id = Guid.NewGuid(),
			   AccountId = accountId,
			   DeviceId = deviceToken,
			   FCMToken = fcmToken
		   };
			return _userDeviceTokenRepository.AddODeviceToken(newDeviceToken);
		}
	}
}
