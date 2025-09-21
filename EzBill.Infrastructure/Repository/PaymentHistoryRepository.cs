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
	public class PaymentHistoryRepository : IPaymentHistoryRepository
	{
		private readonly EzBillDbContext _context;
		public PaymentHistoryRepository(EzBillDbContext context)
		{
			_context = context;
		}
		public async Task<bool> AddPaymentHistoryAsync(PaymentHistory payment)
		{
			try
			{
				await _context.PaymentHistories.AddAsync(payment);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<bool> ChangePaymentStatus(long OrderCode)
		{
			var payment = _context.PaymentHistories.FirstOrDefault(p => p.OrderCode == OrderCode);
			if (payment != null)
			{
				payment.Status = PaymentHistoryStatus.COMPLETED.ToString();
				await	_context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<PaymentHistory?> GetByOrderCode(long OrderCode)
		{
			return await _context.PaymentHistories.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
		}
	}
}
