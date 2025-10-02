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

        Task<List<Settlement>?> GetUnPaidByDebtorIdAsync(Guid debtorId);

		Task<List<Settlement>?> GetUnPaidByCreditorIdAsync(Guid creditor);


		Task<IEnumerable<Settlement>> GetUnpaidSettlementsAsync();

        Task<List<Settlement>> GetAllUnPaidSettlementsByAccountId(Guid accountId);

        Task<bool> ChangeSettlementStatus(Guid settlementId, string status);

        Task<bool> DeleteSettlement(List<Guid> settlements);

        Task<List<Settlement>?> GetAllSettlementsByAccountIdAndMonth(Guid accountId, int month, int year);

        Task<List<Settlement>?> GetAllSettlementNearestMonth(Guid accountId, int months);


	}
}
