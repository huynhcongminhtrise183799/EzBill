using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class TaxRefundRepository : ITaxRefundRepository
    {
        private readonly EzBillDbContext _context;

        public TaxRefundRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaxRefund taxRefund)
        {
            await _context.TaxRefunds.AddAsync(taxRefund);
        }

        public async Task<List<TaxRefund>> GetByTripIdAsync(Guid tripId)
        {
            return await _context.TaxRefunds
                .Include(tr => tr.TaxRefund_Usages)
                .Where(tr => tr.TripId == tripId)
                .ToListAsync();
        }

        public async Task<TaxRefund?> GetByIdAsync(Guid taxRefundId)
        {
            return await _context.TaxRefunds
                .Include(tr => tr.TaxRefund_Usages)
                .FirstOrDefaultAsync(tr => tr.TaxRefundId == taxRefundId);
        }

        public async Task<bool> ExistsAsync(Guid taxRefundId)
        {
            return await _context.TaxRefunds.AnyAsync(tr => tr.TaxRefundId == taxRefundId);
        }

        public async Task<bool> TripExistsAsync(Guid tripId)
        {
            return await _context.trips.AnyAsync(t => t.TripId == tripId);
        }

        public async Task<Guid> GetAnyTripIdAsync()
        {
            var trip = await _context.trips.FirstOrDefaultAsync();
            return trip?.TripId ?? Guid.Empty;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
