using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly EzBillDbContext _context;

        public SettlementRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task<Settlement?> GetByIdAsync(Guid settlementId)
        {
            return await _context.Settlements
                .FirstOrDefaultAsync(s => s.SettlementId == settlementId);
        }

        public async Task<List<Settlement>> GetByTripIdAsync(Guid tripId)
        {
            return await _context.Settlements
                .Include(s => s.FromAccount)
                .Include(s => s.ToAccount)
                .Include(s => s.Trip)
                .Where(s => s.TripId == tripId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Settlement> settlements)
        {
            await _context.Settlements.AddRangeAsync(settlements);
        }

        public async Task UpdateAsync(Settlement settlement)
        {
            _context.Settlements.Update(settlement);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Settlement>?> GetUnPaidByDebtorIdAsync(Guid debtorId)
        {
			return await _context.Settlements
				 .Include(s => s.FromAccount)
				 .Include(s => s.ToAccount)
			.Include(s => s.Trip)
				 .Where(s => s.FromAccountId == debtorId && s.Status == SettlementStatus.UNPAID.ToString())
				 .ToListAsync();
		}

		public async Task<IEnumerable<Settlement>> GetUnpaidSettlementsAsync()
		{
			return await _context.Settlements
				.Include(s => s.FromAccount)
				.Include(s => s.ToAccount)
				.Include(s => s.Trip)
				.Where(s => s.Status == SettlementStatus.UNPAID.ToString())
				.ToListAsync();
		}

		public async Task<List<Settlement>?> GetUnPaidByCreditorIdAsync(Guid creditor)
		{
			return await _context.Settlements
				.Include(s => s.FromAccount)
				.Include(s => s.ToAccount)
				.Include(s => s.Trip)
				.Where(s => s.ToAccountId == creditor && s.Status == SettlementStatus.UNPAID.ToString())
				.ToListAsync();
		}

		public async Task<List<Settlement>> GetAllUnPaidSettlementsByAccountId(Guid accountId)
		{
			return await _context.Settlements
				.Include(s => s.FromAccount)
				.Include(s => s.ToAccount)
				.Include(s => s.Trip)
				.Where(s => (s.FromAccountId == accountId || s.ToAccountId == accountId) && s.Status == SettlementStatus.UNPAID.ToString())
				.ToListAsync();
		}

		public async Task<bool> ChangeSettlementStatus(Guid settlementId, string status)
		{
			var settlement = await _context.Settlements.FirstOrDefaultAsync(s => s.SettlementId == settlementId);
			if (settlement != null)
			{
				settlement.Status = status;
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<bool> DeleteSettlement(List<Guid> settlements)
		{
			var entities = await _context.Settlements
				.Where(s => settlements.Contains(s.SettlementId))
				.ToListAsync();

			if (!entities.Any())
				return false;

			_context.Settlements.RemoveRange(entities);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<List<Settlement>?> GetAllSettlementsByAccountIdAndMonth(Guid accountId, int month, int year)
		{
			var startDate = new DateTime(year, month, 1);
			var endDate = startDate.AddMonths(1).AddDays(-1);

			return await _context.Settlements
				.Include(s => s.FromAccount)
				.Include(s => s.ToAccount)
				.Include(s => s.Trip)
				.Where(s => (s.FromAccountId == accountId || s.ToAccountId == accountId)
							&& s.CreateAt >= startDate && s.CreateAt <= endDate)
				.ToListAsync();
		}

		public async Task<List<Settlement>?> GetAllSettlementNearestMonth(Guid accountId, int months)
		{
			// Tính ngày đầu của tháng hiện tại
			var now = DateTime.UtcNow; // hoặc DateTime.Now nếu bạn muốn theo local time
			var currentMonthStart = new DateTime(now.Year, now.Month, 1);

			// Tính ngày đầu của mốc "months" tháng trước
			var startDate = currentMonthStart.AddMonths(-months + 1);

			// Ngày cuối của tháng hiện tại
			var endDate = currentMonthStart.AddMonths(1).AddDays(-1);

			return await _context.Settlements
				.Include(s => s.FromAccount)
				.Include(s => s.ToAccount)
				.Include(s => s.Trip)
				.Where(s => (s.FromAccountId == accountId || s.ToAccountId == accountId)
							&& s.CreateAt >= startDate && s.CreateAt <= endDate)
				.ToListAsync();
		}

	}
}
