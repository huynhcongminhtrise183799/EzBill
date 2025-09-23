using EzBill.Application.IService;
using EzBill.Domain.IRepository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EzBill.Infrastructure.BackgroundJobs
{
	public class ReminerSettlementService : BackgroundService
	{
		private readonly ILogger<ReminerSettlementService> _logger;
		private readonly ISettlementRepository _settlementRepo;
		private readonly IFirebaseService _firebaseService;

		public ReminerSettlementService(
			ILogger<ReminerSettlementService> logger,
			ISettlementRepository settlementRepo,
			IFirebaseService firebaseService)
		{
			_logger = logger;
			_settlementRepo = settlementRepo;
			_firebaseService = firebaseService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("ReminerSettlementService started.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					// 1. Lấy danh sách các settlement chưa thanh toán
					var unpaidSettlements = await _settlementRepo.GetUnpaidSettlementsAsync();

					if (unpaidSettlements != null && unpaidSettlements.Any())
					{
						_logger.LogInformation("Found {Count} unpaid settlements. Sending reminders...", unpaidSettlements.Count());

						// 2. Gửi nhắc nợ qua Firebase
						var responses = await _firebaseService.SendDebtReminderAsync(unpaidSettlements);

						foreach (var res in responses)
						{
							_logger.LogInformation("Debt reminder result: {Result}", res);
						}
					}
					else
					{
						_logger.LogInformation("No unpaid settlements found at {Time}", DateTimeOffset.Now);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error while executing ReminerSettlementService");
				}

				// 3. Đợi trước khi chạy vòng lặp tiếp theo (ví dụ: mỗi 1 giờ)
				await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
			}

			_logger.LogInformation("ReminerSettlementService stopped.");
		}
	}
}
