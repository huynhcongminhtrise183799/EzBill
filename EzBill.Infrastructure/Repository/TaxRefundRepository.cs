using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class TaxRefundRepository : ITaxRefundRepository
    {
        private readonly EzBillDbContext _context;

        public TaxRefundRepository(EzBillDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task AddAsync(TaxRefund taxRefund)
        {
            if (taxRefund == null)
                throw new ArgumentNullException(nameof(taxRefund));

            await _context.TaxRefunds.AddAsync(taxRefund);

            if (taxRefund.TaxRefund_Usages != null && taxRefund.TaxRefund_Usages.Any())
            {
                foreach (var usage in taxRefund.TaxRefund_Usages)
                {
                    if (usage.TaxRefundId == Guid.Empty)
                        usage.TaxRefundId = taxRefund.TaxRefundId;

                    await _context.TaxRefund_Usages.AddAsync(usage);
                }
            }
        }

        public async Task<TaxRefund?> GetByIdAsync(Guid taxRefundId)
        {
            if (taxRefundId == Guid.Empty)
                throw new ArgumentException("TaxRefundId không hợp lệ.", nameof(taxRefundId));

            return await _context.TaxRefunds
                .Include(t => t.TaxRefund_Usages)
                .ThenInclude(u => u.Account)
                .FirstOrDefaultAsync(t => t.TaxRefundId == taxRefundId);
        }

        public async Task<bool> EventExistsAsync(Guid eventId)
        {
            return await _context.Events.AnyAsync(e => e.EventId == eventId);
        }

        public async Task<Guid> GetAnyEventIdAsync()
        {
            var anyEvent = await _context.Events.FirstOrDefaultAsync();
            return anyEvent?.EventId ?? Guid.Empty;
        }

        public async Task<bool> ExistsAsync(Guid taxRefundId)
        {
            return await _context.TaxRefunds.AnyAsync(t => t.TaxRefundId == taxRefundId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
