using EzBill.Application.DTO.Settlement;
using EzBill.Domain.Entity;


namespace EzBill.Application.IService
{
    public interface ISettlementService
    {
        Task<List<SettlementResultDto>> GenerateSettlementsAsync(Guid tripId);
        Task<List<SettlementResultDto>> GetSettlementsByTripAsync(Guid tripId);
        Task UpdateSettlementStatusAsync(Guid settlementId, SettlementStatus status);
    }
}
