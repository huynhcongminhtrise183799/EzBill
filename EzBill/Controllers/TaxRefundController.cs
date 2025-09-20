using EzBill.Application.DTO.TaxRefund;
using EzBill.Application.IService;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Process([FromBody] TaxRefundRequestDto request)
        {
            var result = await _taxRefundService.ProcessTaxRefundAsync(request);
            return Ok(result);
        }

        [HttpGet("trip/{tripId}")]
        public async Task<IActionResult> GetByTrip(Guid tripId)
        {
            var result = await _taxRefundService.GetTaxRefundsByTripAsync(tripId);
            return Ok(result);
        }
    }
}
