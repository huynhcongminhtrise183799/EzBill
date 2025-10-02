using EzBill.Application.DTO.Settlement;
using EzBill.Application.ServiceModel.Settlement;
using EzBill.Domain.Entity;


namespace EzBill.Application.IService
{
    public interface ISettlementService
    {
        Task<List<SettlementResultDto>> GenerateSettlementsAsync(Guid tripId);
        Task<List<SettlementResultDto>> GetSettlementsByTripAsync(Guid tripId);
        Task UpdateSettlementStatusAsync(Guid settlementId, SettlementStatus status);

        Task<List<Settlement>?> GetByDebtorIdAsync(Guid debtorId);

		Task<List<SettlementResultDto>> GetSettlementsByAccountIdWithFiltterAsync(Guid accountId, string state);

        Task<bool> ChangeSettlementStatusToPaid(Guid settlementId);

        Task<SettlementStatisticsModel?> GetAllSettlementByMonthAndAccount(Guid accountId, int month, int year);

		Task<List<SettlementStatisticsModel>?> GetAllSettlementNearestMonthByAccount(Guid accountId, int months);


	}
}
