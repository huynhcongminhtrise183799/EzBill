using EzBill.Application.IService;
using EzBill.Domain.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EzBill.Infrastructure.BackgroundJobs
{
	public class ReminerSettlementService : BackgroundService
	{
		private readonly ILogger<ReminerSettlementService> _logger;
		private readonly IServiceScopeFactory _scopeFactory;

		public ReminerSettlementService(
			ILogger<ReminerSettlementService> logger,
			IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("ReminerSettlementService started.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (var scope = _scopeFactory.CreateScope())
					{
						var settlementRepo = scope.ServiceProvider.GetRequiredService<ISettlementRepository>();
						var firebaseService = scope.ServiceProvider.GetRequiredService<IFirebaseService>();

						// 1. Lấy danh sách các settlement chưa thanh toán
						var unpaidSettlements = await settlementRepo.GetUnpaidSettlementsAsync();

						if (unpaidSettlements != null && unpaidSettlements.Any())
						{
							_logger.LogInformation("Found {Count} unpaid settlements. Sending reminders...", unpaidSettlements.Count());

							// 2. Gửi nhắc nợ qua Firebase
							var responses = await firebaseService.SendDebtReminderAsync(unpaidSettlements);

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
