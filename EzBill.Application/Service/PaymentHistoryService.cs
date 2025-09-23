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

		public async Task<PaymentHistory?> GetByOrderCode(long OrderCode)
		{
			return await _paymentHistoryRepository.GetByOrderCode(OrderCode);
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
