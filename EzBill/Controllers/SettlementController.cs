using EzBill.Application.IService;
using EzBill.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzBill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSettlements([FromBody] Guid tripId)
        {
            var result = await _settlementService.GenerateSettlementsAsync(tripId);
            return Ok(result);
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetByTrip(Guid tripId)
        {
            var result = await _settlementService.GetSettlementsByTripAsync(tripId);
            return Ok(result);
        }

        [HttpPut("{settlementId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid settlementId, [FromQuery] SettlementStatus status)
        {
            await _settlementService.UpdateSettlementStatusAsync(settlementId, status);
            return NoContent();
        }


    }
}
