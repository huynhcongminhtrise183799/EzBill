using EzBill.Application.DTO;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxRefundController : ControllerBase
    {
        private readonly ITaxRefundService _taxRefundService;

        public TaxRefundController(ITaxRefundService taxRefundService)
        {
            _taxRefundService = taxRefundService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessTaxRefund([FromBody] TaxRefundRequestDto dto)
        {
            try
            {
                var taxRefund = new TaxRefund
                {
                    TripId =dto.TripId,
                    ProductName = dto.ProductName,
                    OriginalAmount = dto.OriginalAmount,
                    RefundPercent = dto.RefundPercent,
                    RefundedBy = dto.RefundedBy,
                    SplitType = dto.SplitType,
                    TaxRefund_Usages = dto.TaxRefund_Usages.Select(u => new TaxRefund_Usage
                    {
                        AccountId = u.AccountId,
                        Ratio = u.Ratio
                    }).ToList()
                };

                await _taxRefundService.ProcessTaxRefundAsync(taxRefund);

                var result = new
                {
                    Message = "Hoàn thuế thành công!",
                    ProductName = taxRefund.ProductName,
                    OriginalAmount = $"{taxRefund.OriginalAmount:N0}₫",
                    RefundPercent = $"{taxRefund.RefundPercent}%",
                    RefundAmount = $"{taxRefund.RefundAmount:N0}₫",
                    SplitType = taxRefund.SplitType.ToString(),
                    Beneficiaries = taxRefund.TaxRefund_Usages.Select(u => new
                    {
                        AccountId = u.AccountId,
                        Ratio = $"{u.Ratio}%",
                        AmountReceived = $"{u.AmountReceived:N0}₫"
                    })
                };

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = $"Xử lý thất bại: {ex.Message}" });
            }
        }
        [HttpGet("by-trip/{tripId}")]
        public async Task<IActionResult> GetTaxRefundsByTrip(Guid tripId)
        {
            var result = await _taxRefundService.GetTaxRefundsByTripAsync(tripId);
            return Ok(result);
        }
    }
}
