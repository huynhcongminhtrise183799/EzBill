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

		public async Task<bool> ChangePaymentStatus(long OrderCode, string status)
		{
			var payment = _context.PaymentHistories.FirstOrDefault(p => p.OrderCode == OrderCode);
			if (payment != null)
			{
				payment.Status = status;
				await	_context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<List<PaymentHistory>> GetAllPaymentHistory()
		{
			return await _context.PaymentHistories.Include(p => p.Plan).Include(p => p.FromAccount)
				.ToListAsync();
		}

		// Đổi tên hàm một chút để rõ nghĩa hơn (tùy chọn)
		public Task<List<PaymentHistory>?> GetAllPaymentNearestMonth(int months)
		{
			var now = DateTime.UtcNow; // hoặc DateTime.Now nếu bạn muốn theo local time
			var currentMonthStart = new DateTime(now.Year, now.Month, 1);

			// Tính ngày đầu của mốc "months" tháng trước
			var startDate = currentMonthStart.AddMonths(-months + 1);

			// Ngày cuối của tháng hiện tại
			var endDate = currentMonthStart.AddMonths(1).AddDays(-1);

			return _context.PaymentHistories
				.Where(p => p.PaymentDate.ToDateTime(new TimeOnly(0, 0)) >= startDate &&
							p.PaymentDate.ToDateTime(new TimeOnly(0, 0)) <= endDate &&
							p.Status == "COMPLETED") // <-- Thêm điều kiện lọc status
				.ToListAsync();
		}

		public async Task<PaymentHistory?> GetByOrderCode(long OrderCode)
		{
			return await _context.PaymentHistories.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
		}
	}
}
