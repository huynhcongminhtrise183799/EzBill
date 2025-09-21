using EzBill.Application.IService;
using EzBill.Application.Service;
using EzBill.Domain.IRepository;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.ExternalService
{
	public class FirebaseService : IFirebaseService
	{
		private readonly IUserDeviceTokenService _userDeviceTokenService;
		private readonly ISettlementRepository _settlementRepo;
		private readonly IAccountRepository _accountRepo;

		public FirebaseService(IUserDeviceTokenService userDeviceTokenService, ISettlementRepository settlementRepo, IAccountRepository accountRepo)
		{
			_userDeviceTokenService = userDeviceTokenService;
			_settlementRepo = settlementRepo;
			_accountRepo = accountRepo;
		}
		public async Task<List<string>> SendNotificationToAccountsAsync(Guid tripId)
		{
			var settlements = await _settlementRepo.GetByTripIdAsync(tripId);

			var responses = new List<string>();

			foreach (var settlement in settlements)
			{
				var fromAccountId = settlement.FromAccountId;
				var toAccountId = settlement.ToAccountId;
				var amount = settlement.Amount;
				var fromNickName = settlement.FromAccount.NickName;
				var toNickName = settlement.ToAccount.NickName;

				// Lấy tất cả token của người nợ
				var fromTokens = await _userDeviceTokenService.GetDeviceTokensByAccountId(fromAccountId);
				if (fromTokens?.Any() == true)
				{
					var fromMessage = new MulticastMessage
					{
						Tokens = fromTokens, // danh sách token
						Notification = new Notification
						{
							Title = "Thông báo nợ",
							Body = $"Bạn nợ {toNickName} {amount:N0}đ"
						}
					};

					var fromResponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(fromMessage);
					responses.Add($"fromNickName={fromNickName}, Success={fromResponse.SuccessCount}, Fail={fromResponse.FailureCount}");
				}

				// Lấy tất cả token của người được nợ
				var toTokens = await _userDeviceTokenService.GetDeviceTokensByAccountId(toAccountId);
				if (toTokens?.Any() == true)
				{
					var toMessage = new MulticastMessage
					{
						Tokens = toTokens, // danh sách token
						Notification = new Notification
						{
							Title = "Thông báo nợ",
							Body = $"{fromNickName} nợ bạn {amount:N0}đ"
						}
					};

					var toResponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(toMessage);
					responses.Add($"toNickName={toNickName}, Success={toResponse.SuccessCount}, Fail={toResponse.FailureCount}");
				}
			}

			return responses;
		}


		public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
		{
			try
			{
				var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
				return decodedToken;
			}
			catch (Exception ex)
			{
				throw new UnauthorizedAccessException("Invalid Firebase token", ex);
			}
		}
	}
}
