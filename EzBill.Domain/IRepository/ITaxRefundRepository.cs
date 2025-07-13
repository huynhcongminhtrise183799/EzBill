using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
    public interface ITaxRefundRepository
    {
        Task AddAsync(TaxRefund taxRefund);
        Task<TaxRefund?> GetByIdAsync(Guid taxRefundId);
        Task<bool> ExistsAsync(Guid taxRefundId);
        Task<bool> EventExistsAsync(Guid eventId);
        Task<Guid> GetAnyEventIdAsync();
        Task SaveChangesAsync();
    }
}
