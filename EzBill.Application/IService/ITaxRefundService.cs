using EzBill.Application.DTO.TaxRefund;

namespace EzBill.Application.IService
{
    public interface ITaxRefundService
    {
        Task<List<TaxRefundResponseDto>> ProcessTaxRefundAsync(TaxRefundRequestDto request);
        Task<List<TaxRefundResponseDto>> GetTaxRefundsByTripAsync(Guid tripId);
    }
}
