using EzBill.Application.DTO.Settlement;
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
        public async Task<IActionResult> GenerateSettlements([FromBody] GenerateSettlementRequest request)
        {
            var result = await _settlementService.GenerateSettlementsAsync(request.TripId);
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

        [HttpGet("debtor/{id}")]
        public async Task<IActionResult> GetSettlementsByDebtor(Guid id)
        {
            var result = await _settlementService.GetByDebtorIdAsync(id);
            if (result == null || result.Count == 0)
            {
                return BadRequest("No settlements found for the specified debtor.");
            }
            return Ok(result);
        }

    }
}
