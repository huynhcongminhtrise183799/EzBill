using EzBill.Application.IService;
using EzBill.Application.Service;
using EzBill.Domain.Entity;
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
		public async Task<List<string>> SendDebtReminderAsync(IEnumerable<Settlement> unpaidSettlements)
		{
			var responses = new List<string>();
			var random = new Random();

			// Đọc tất cả câu template từ file
			var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "debt_messages.txt");

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Debt message file not found", filePath);
			}

			var messageTemplates = await File.ReadAllLinesAsync(filePath);
			foreach (var settlement in unpaidSettlements)
			{
				var fromAccountId = settlement.FromAccountId;
				var toAccountId = settlement.ToAccountId;
				var amount = settlement.Amount;
				var fromNickName = settlement.FromAccount?.NickName ?? "Người nợ";
				var toNickName = settlement.ToAccount?.NickName ?? "Người cho vay";
				var tripName = settlement.Trip?.TripName ?? "Chuyến đi";

				string debtorMessage = messageTemplates[random.Next(messageTemplates.Length)];
				debtorMessage = debtorMessage
					.Replace("{fromNickName}", fromNickName)
					.Replace("{toNickName}", toNickName)
					.Replace("{tripName}", tripName)
					.Replace("{amount:N0}", amount.ToString("N0"));

				// Gửi cho người nợ
				var fromTokens = await _userDeviceTokenService.GetDeviceTokensByAccountId(fromAccountId);
				if (fromTokens?.Any() == true)
				{
					var fromMessage = new MulticastMessage
					{
						Tokens = fromTokens,
						Notification = new Notification
						{
							Title = "Nhắc nợ",
							Body = debtorMessage
						}
					};

					var fromResponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(fromMessage);
					responses.Add($"DebtReminder->Debtor={fromNickName}, Success={fromResponse.SuccessCount}, Fail={fromResponse.FailureCount}");
				}

				// Gửi cho người được nợ (người cho vay)
				var toTokens = await _userDeviceTokenService.GetDeviceTokensByAccountId(toAccountId);
				if (toTokens?.Any() == true)
				{
					var creditorMessage = $"{fromNickName} vẫn đang nợ bạn {amount:N0}đ trong chuyến đi {tripName}";
					var toMessage = new MulticastMessage
					{
						Tokens = toTokens,
						Notification = new Notification
						{
							Title = "Thông báo nợ",
							Body = creditorMessage
						}
					};

					var toResponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(toMessage);
					responses.Add($"DebtReminder->Creditor={toNickName}, Success={toResponse.SuccessCount}, Fail={toResponse.FailureCount}");
				}
			}

			return responses;
		}


		public async Task<List<string>> SendNotiConfirmedAsync(Guid toAccountId)
		{
			var responses = new List<string>();

			var tokens = await _userDeviceTokenService.GetDeviceTokensByAccountId(toAccountId);

			if (tokens == null || !tokens.Any())
			{
				responses.Add($"No device tokens found for account {toAccountId}");
				return responses;
			}

			var message = new MulticastMessage
			{
				Tokens = tokens,
				Notification = new Notification
				{
					Title = "Thanh toán thành công 🎉",
					Body = "Khoản nợ của bạn đã được xác nhận là đã thanh toán."
				}
			};

			var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

			for (int i = 0; i < response.Responses.Count; i++)
			{
				var result = response.Responses[i];
				var token = tokens[i];

				if (result.IsSuccess)
				{
					responses.Add($"Token={token}, Success: MessageId={result.MessageId}");
				}
				else
				{
					responses.Add($"Token={token}, Fail: {result.Exception?.Message}");
				}
			}

			return responses;
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
