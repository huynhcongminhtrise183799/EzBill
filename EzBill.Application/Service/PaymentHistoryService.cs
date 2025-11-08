using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Payment;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class PaymentHistoryService : IPaymentHistoryService
	{
		private readonly IPaymentHistoryRepository _paymentHistoryRepository;
		public PaymentHistoryService(IPaymentHistoryRepository paymentHistoryRepository)
		{
			_paymentHistoryRepository = paymentHistoryRepository;
		}
		public Task<bool> AddPaymentHistoryAsync(CreatePaymentHistoryModel payment)
		{
			DateTime now = DateTime.UtcNow;

			DateOnly dateOnly = DateOnly.FromDateTime(now);
			var paymentHistory = new PaymentHistory
			{
				PaymentHistoryId = payment.PaymentHistoryModelId,
				OrderCode = payment.OrderCode,
				PlanId = payment.PlanId,
				FromAccountId = payment.FromAccountId,
				Amount = payment.Amount,
				PaymentDate = dateOnly,
				PaymentType = PaymentHistoryPaymentType.BUY_PLAN.ToString(),
				Status = PaymentHistoryStatus.PENDING.ToString()
			};

			return _paymentHistoryRepository.AddPaymentHistoryAsync(paymentHistory);
		}

		public async Task<bool> ChangePaymentStatus(long OrderCode, string status)
		{
			return await _paymentHistoryRepository.ChangePaymentStatus(OrderCode, status);
		}

		public async Task<List<PaymentHistoryModel>> GetAll()
		{
			var payments = await _paymentHistoryRepository.GetAllPaymentHistory();
			var paymentModels = payments.Select(p => new PaymentHistoryModel
			{
				PaymentHistoryModelId = p.PaymentHistoryId,
				UserId = p.FromAccount.AccountId,
				PlanId = (Guid)p.PlanId,
				Email = p.FromAccount.Email,
				PlanName = p.Plan.PlanName,
				Amount = p.Amount,
				PaymentDate = p.PaymentDate,
				Status = p.Status
			}).ToList();
			return paymentModels;

		}

		public async Task<PaymentHistory?> GetByOrderCode(long OrderCode)
		{
			return await _paymentHistoryRepository.GetByOrderCode(OrderCode);
		}

		public async Task<List<MonthlyPaymentSummary>> GetCompletedMonthlySummary(int months)
		{
			// 1. GỌI REPO: Lấy danh sách các Entities
			var payments = await _paymentHistoryRepository.GetAllPaymentNearestMonth(months);

			// 2. LOGIC & MAP: Thực hiện GroupBy và Map sang Model (DTO)
			// Đây là LINQ to Objects (chạy trên bộ nhớ của server)
			var summary = payments
				.GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
				.Select(g => new MonthlyPaymentSummary
				{
					Year = g.Key.Year,
					Month = g.Key.Month,
					TotalAmount = g.Sum(p => p.Amount) // <-- THAY THẾ 'Amount'
				})
				.OrderBy(s => s.Year)
				.ThenBy(s => s.Month)
				.ToList(); // Dùng .ToList() vì 'payments' đã là List in-memory

			return summary;
		}

		public async Task<string> GetPaymentStatusByOrderCode(long OrderCode)
		{
			var payment = await _paymentHistoryRepository.GetByOrderCode(OrderCode);
			if (payment == null)
			{
				throw new AppException("Payment not found", 404);
			}
			return payment.Status;
		}
	}
}
