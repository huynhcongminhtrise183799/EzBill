using EzBill.Domain.Entity;

namespace EzBill.Domain.IRepository
{
    public interface ISettlementRepository
    {
        Task<Settlement?> GetByIdAsync(Guid settlementId);
        Task<List<Settlement>> GetByTripIdAsync(Guid tripId);
        Task AddRangeAsync(List<Settlement> settlements);
        Task UpdateAsync(Settlement settlement);
        Task SaveChangesAsync();
    }
}
